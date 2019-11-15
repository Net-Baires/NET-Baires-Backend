using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Features.Badges.NewBadge;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Badges
{
    public class NewBadgeShould : IntegrationTestsBase
    {
        public NewBadgeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }


        [Fact]
        public async Task Add_New_Badge()
        {
            var file1 = File.OpenRead(@"Images\Blanco.png");
            var badgeFile = new StreamContent(file1);
            badgeFile.Headers.Add("BadgeType", BadgeImageName.Badge.ToString());
            var simpleBadgeFile = new StreamContent(file1);
            simpleBadgeFile.Headers.Add("BadgeType", BadgeImageName.SimpleBadge.ToString());

            var formData = new MultipartFormDataContent();

            formData.Add(badgeFile, nameof(NewBadgeCommand.ImageFiles), $"{BadgeImageName.Badge}.jpg");
            formData.Add(simpleBadgeFile, nameof(NewBadgeCommand.ImageFiles), $"{BadgeImageName.SimpleBadge}.jpg");
            formData.Add(new StringContent(nameof(NewBadgeCommand.Name)), "Name");
            formData.Add(new StringContent(nameof(NewBadgeCommand.Description)), "Description");

            var response = await HttpClient.PostAsync("/badges", formData);



            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var badge = Context.Badges.FirstOrDefault();
            badge.Name.Should().Be("Name");
            badge.Created.Date.Should().Be(DateTime.Now.Date);
            badge.Description.Should().Be("Description");
            (await FileServices.GetAsync(badge.ImageName, Api.Services.Container.Badges)).Should().NotBeNull();
            (await FileServices.GetAsync(badge.SimpleImageName, Api.Services.Container.Badges)).Should().NotBeNull();

            await FileServices.DeleteAsync(badge.ImageName, Api.Services.Container.Badges);
            await FileServices.DeleteAsync(badge.SimpleImageName, Api.Services.Container.Badges);
        }
      
    }
}