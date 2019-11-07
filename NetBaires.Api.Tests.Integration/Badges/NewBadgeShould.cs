using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure;
using System.Net.Http;
using FluentAssertions;
using System.Net;
using System.Collections.Generic;
using static NetBaires.Api.Handlers.Badges.GetBadeHandler;
using System.Linq;
using System;
using NetBaires.Api.Handlers.Badges;
using System.IO;

namespace NetBaires.Api.Tests.Integration
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
            // Add file (file, field name, file name)
            formData.Add(badgeFile, nameof(NewBadgeHandler.NewBadge.ImageFiles), $"{BadgeImageName.Badge}.jpg");
            formData.Add(simpleBadgeFile, nameof(NewBadgeHandler.NewBadge.ImageFiles), $"{BadgeImageName.SimpleBadge}.jpg");
            formData.Add(new StringContent(nameof(NewBadgeHandler.NewBadge.Name)), "Name");
            formData.Add(new StringContent(nameof(NewBadgeHandler.NewBadge.Description)), "Description");
            var response = await HttpClient.PostAsync("/badges", formData);



            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var badge = Context.Badges.FirstOrDefault();
            badge.Name.Should().Be("Name");
            badge.Created.Date.Should().Be(DateTime.Now.Date);
            badge.Description.Should().Be("Description");
            (await FileServices.GetAsync(badge.ImageName, Services.Container.Badges)).Should().NotBeNull();
            (await FileServices.GetAsync(badge.SimpleImageName, Services.Container.Badges)).Should().NotBeNull();

            await FileServices.DeleteAsync(badge.ImageName, Services.Container.Badges);
            await FileServices.DeleteAsync(badge.SimpleImageName, Services.Container.Badges);
        }
      
    }
}