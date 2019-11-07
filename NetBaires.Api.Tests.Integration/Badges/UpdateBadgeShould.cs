using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using FluentAssertions;
using System.Net;
using System.Linq;
using System;
using NetBaires.Api.Handlers.Badges;
using System.IO;
using NetBaires.Data;

namespace NetBaires.Api.Tests.Integration
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
            var formData = new MultipartFormDataContent();
            formData.Add(badgeFile, nameof(UpdateBadgeHandler.UpdateBadge.ImageFiles), $"{BadgeImageName.Badge}.jpg");
            formData.Add(simpleBadgeFile, nameof(UpdateBadgeHandler.UpdateBadge.ImageFiles), $"{BadgeImageName.SimpleBadge}.jpg");
            formData.Add(new StringContent(nameof(UpdateBadgeHandler.UpdateBadge.Name)), "New Name");
            formData.Add(new StringContent(nameof(UpdateBadgeHandler.UpdateBadge.Description)), "New Description");
            var response = await HttpClient.PutAsync($"/badges/{badge.Id}", formData);

            var oldImageName = badge.ImageName;
            var oldSimpleImageName = badge.SimpleImageName;
            Context.Entry(badge).Reload();

            //newBadge.Name.Should().Be("New Name");
            //newBadge.Description.Should().Be("New Description");
            (await FileServices.GetAsync(oldImageName, Services.Container.Badges)).Should().BeNull();
            (await FileServices.GetAsync(oldSimpleImageName, Services.Container.Badges)).Should().BeNull();

            (await FileServices.GetAsync(badge.ImageName, Services.Container.Badges)).Should().NotBeNull();
            (await FileServices.GetAsync(badge.SimpleImageName, Services.Container.Badges)).Should().NotBeNull();

            await FileServices.DeleteAsync(badge.ImageName, Services.Container.Badges);
            await FileServices.DeleteAsync(badge.SimpleImageName, Services.Container.Badges);
        }
        public void RefreshAll()
        {
            foreach (var entity in Context.ChangeTracker.Entries())
            {
                entity.Reload();
            }
        }
        private async Task AddNewBadge()
        {
            var file1 = File.OpenRead(@"Images\Blanco.png");
            var badgeFile = new StreamContent(file1);
            badgeFile.Headers.Add("BadgeType", BadgeImageName.Badge.ToString());
            var simpleBadgeFile = new StreamContent(file1);
            simpleBadgeFile.Headers.Add("BadgeType", BadgeImageName.SimpleBadge.ToString());
            var formData = new MultipartFormDataContent();
            formData.Add(badgeFile, nameof(NewBadgeHandler.NewBadge.ImageFiles), $"{BadgeImageName.Badge}.jpg");
            formData.Add(simpleBadgeFile, nameof(NewBadgeHandler.NewBadge.ImageFiles), $"{BadgeImageName.SimpleBadge}.jpg");
            formData.Add(new StringContent(nameof(NewBadgeHandler.NewBadge.Name)), "Name");
            formData.Add(new StringContent(nameof(NewBadgeHandler.NewBadge.Description)), "Description");
            var response = await HttpClient.PostAsync("/badges", formData);
        }
    }
}