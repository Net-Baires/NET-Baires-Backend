using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.ViewModels;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Badges
{
    public class GetBadgesShould : IntegrationTestsBase
    {
        public GetBadgesShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_All_Badges()
        {
            var dateTimeNow = new DateTime(2019, 10, 10, 12, 12, 12);
            var firstBadgeToAdd = new Data.Badge
            {
                Name = "First Badge",
                Description = "First badge description",
                Created = dateTimeNow,
                ImageName = "TestImage",
                ImageUrl="http://imageurltest.com",
                SimpleImageUrl="https://simpleimage.url.com"
            };
            Context.Badges.Add(firstBadgeToAdd);
            Context.Badges.Add(new Data.Badge
            {
                Name = "First Badge",
                Description = "First badge description",
                ImageUrl = "http://imageurltest.com",
                SimpleImageUrl = "https://simpleimage.url.com"
            });
            Context.SaveChanges();
            var response = await HttpClient.GetAsync("/badges");

            var badges = await response.Content.ReadAsAsync<List<BadgeDetailViewModel>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            badges.Count.Should().Be(2);
            badges.First().Name.Should().Be(firstBadgeToAdd.Name);
            badges.First().Description.Should().Be(firstBadgeToAdd.Description);
            badges.First().ImageUrl.Should().Contain(firstBadgeToAdd.ImageUrl);
            badges.First().SimpleImageUrl.Should().Contain(firstBadgeToAdd.SimpleImageUrl);
            badges.First().Created.Should().Be(dateTimeNow);
        }

    }
}