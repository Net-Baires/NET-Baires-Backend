using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Features.Badges.NewBadge;
using NetBaires.Api.Features.Badges.UpdateBadge;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Badges
{
    public class UpdateBadgeShould : IntegrationTestsBase
    {
        public UpdateBadgeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }


        [Fact]
        public async Task Update_Badge()
        {
            await AddNewBadge();

            var badge = Context.Badges.FirstOrDefault();

            var file1 = File.OpenRead(@"Images\Blanco.png");
            var badgeFile = new StreamContent(file1);
            badgeFile.Headers.Add("BadgeType", BadgeImageName.Badge.ToString());
            var simpleBadgeFile = new StreamContent(file1);
            simpleBadgeFile.Headers.Add("BadgeType", BadgeImageName.SimpleBadge.ToString());
            var linkedinBadgeFile = new StreamContent(file1);
            linkedinBadgeFile.Headers.Add("BadgeType", BadgeImageName.LinkedinBadge.ToString());

            var formData = new MultipartFormDataContent();
            formData.Add(badgeFile, nameof(UpdateBadgeCommand.ImageFiles), $"{BadgeImageName.Badge}.jpg");
            formData.Add(simpleBadgeFile, nameof(UpdateBadgeCommand.ImageFiles), $"{BadgeImageName.SimpleBadge}.jpg");
            formData.Add(linkedinBadgeFile, nameof(UpdateBadgeCommand.ImageFiles), $"{BadgeImageName.LinkedinBadge}.jpg");
            formData.Add(new StringContent("New Name"), nameof(UpdateBadgeCommand.Name));
            formData.Add(new StringContent("New Description"), nameof(UpdateBadgeCommand.Description));

            await HttpClient.PutAsync($"/badges/{badge.Id}", formData);

            var oldImageName = badge.ImageName;
            var oldSimpleImageName = badge.SimpleImageName;
            Context.Entry(badge).Reload();
            badge.Name.Should().Be("New Name");
            badge.Description.Should().Be("New Description");

            (await FileServices.GetAsync(oldImageName, Api.Services.Container.Badges)).Should().BeNull();
            (await FileServices.GetAsync(oldSimpleImageName, Api.Services.Container.Badges)).Should().BeNull();

            (await FileServices.GetAsync(badge.ImageName, Api.Services.Container.Badges)).Should().NotBeNull();
            (await FileServices.GetAsync(badge.SimpleImageName, Api.Services.Container.Badges)).Should().NotBeNull();
            (await FileServices.GetAsync(badge.LinkedinImageName, Api.Services.Container.Badges)).Should().NotBeNull();

            await FileServices.DeleteAsync(badge.ImageName, Api.Services.Container.Badges);
            await FileServices.DeleteAsync(badge.SimpleImageName, Api.Services.Container.Badges);
            await FileServices.DeleteAsync(badge.LinkedinImageName, Api.Services.Container.Badges);
        }

        private async Task AddNewBadge()
        {
            var file1 = File.OpenRead(@"Images\Blanco.png");
            var badgeFile = new StreamContent(file1);
            badgeFile.Headers.Add("BadgeType", BadgeImageName.Badge.ToString());
            var simpleBadgeFile = new StreamContent(file1);
            simpleBadgeFile.Headers.Add("BadgeType", BadgeImageName.SimpleBadge.ToString());

            var linkedinBadgeFile = new StreamContent(file1);
            linkedinBadgeFile.Headers.Add("BadgeType", BadgeImageName.LinkedinBadge.ToString());

            var formData = new MultipartFormDataContent();
            formData.Add(badgeFile, nameof(NewBadgeCommand.ImageFiles), $"{BadgeImageName.Badge}.jpg");
            formData.Add(simpleBadgeFile, nameof(NewBadgeCommand.ImageFiles), $"{BadgeImageName.SimpleBadge}.jpg");
            formData.Add(linkedinBadgeFile, nameof(NewBadgeCommand.ImageFiles), $"{BadgeImageName.LinkedinBadge}.jpg");
            formData.Add(new StringContent("Name"), nameof(NewBadgeCommand.Name));
            formData.Add(new StringContent("Description"), nameof(NewBadgeCommand.Description));
            var response = await HttpClient.PostAsync("/badges", formData);
        }
    }
}