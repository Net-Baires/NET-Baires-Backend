using FluentAssertions;
using NetBaires.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NetBaires.Api.ViewModels;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Members
{
    public class GetBadgesFromEmailShould : IntegrationTestsBase
    {
        public GetBadgesFromEmailShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_Badges_From_Member_By_Email()
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

            var response = await HttpClient.GetAsync($"/members/badges?email={member.Email}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var badges = await response.Content.ReadAsAsync<List<BadgeMemberViewModel>>();
            badges.Count.Should().Be(2);
            badges.First().Badge.Name.Should().Be("Test Badge");
            badges[1].Badge.Name.Should().Be("Test Badge 2");
        }

        [Fact]
        public async Task Return_Badges_From_Member_By_Id()
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

            var response = await HttpClient.GetAsync($"/members/{member.Id}/badges");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var badges = await response.Content.ReadAsAsync<List<BadgeMemberViewModel>>();
            badges.Count.Should().Be(2);
            badges.First().Badge.Name.Should().Be("Test Badge");
            badges[1].Badge.Name.Should().Be("Test Badge 2");
        }

        [Fact]
        public async Task Return_NoContent()
        {
            var response = await HttpClient.GetAsync($"/members/badges?email=test@test.com");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
