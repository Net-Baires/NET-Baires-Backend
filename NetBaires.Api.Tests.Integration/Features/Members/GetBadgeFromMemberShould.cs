using FluentAssertions;
using NetBaires.Data;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NetBaires.Api.ViewModels;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Members
{
    public class GetBadgeFromMemberShould : IntegrationTestsBase
    {
        public GetBadgeFromMemberShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_Badge_From_Member()
        {
            var newBadge = new Badge
            {
                Name = "Test Badge"
            };
            var member = new Member
            {
                Email = "Test@test.com",
                Badges = new List<BadgeMember>{
                                   new BadgeMember{
                                       Badge= newBadge
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

            var response = await HttpClient.GetAsync($"/members/{member.Id}/badges/{newBadge.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var badge = await response.Content.ReadAsAsync<BadgeMemberViewModel>();
            badge.Badge.Name.Should().Be(newBadge.Name);
            badge.Badge.Id.Should().Be(newBadge.Id);
        }

        [Fact]
        public async Task Return_404_Member_Has_Not_Badge()
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


            var response = await HttpClient.GetAsync($"/members/{member.Id}/badges/{99}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
