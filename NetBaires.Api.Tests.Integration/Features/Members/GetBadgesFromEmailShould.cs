using FluentAssertions;
using NetBaires.Api.Features.Badges.Models;
using NetBaires.Api.Handlers.Events;
using NetBaires.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.BadgeGroups
{
    public class GetBadgesFromEmailShould : IntegrationTestsBase
    {
        public GetBadgesFromEmailShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_Badges_From_Member()
        {
            var member = new Member
            {
                Email = "Test@test.com",
                Badges = new List<BadgeMember>{
                                   new BadgeMember{
                                       Badge= new Badge
                                       {
                                           Name="Test Badge"
                                       }
                                   },
                                   new BadgeMember{
                                       Badge= new Badge
                                       {
                                           Name="Test Badge 2"
                                       }
                                   }
               }
            };
            await Context.Members.AddAsync(member);
            await Context.SaveChangesAsync();

            var response = await HttpClient.GetAsync($"/members/{member.Email}/badges");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var badges = await response.Content.ReadAsAsync<List<BadgeDetailViewModel>>();
            badges.Count.Should().Be(2);
            badges.First().Name.Should().Be("Test Badge");
            badges[1].Name.Should().Be("Test Badge 2");
        }

        [Fact]
        public async Task Return_NoContent()
        {
            var response = await HttpClient.GetAsync($"/members/test@test.com/badges");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
