using FluentAssertions;
using NetBaires.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Members.GetFollowingsFromMember;
using NetBaires.Api.ViewModels;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Members
{
    public class GetFollowingsFromMemberShould : IntegrationTestsBase
    {
        public GetFollowingsFromMemberShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_All_Followings()
        {
            var memberToFollow = new Member();
            Context.Members.Add(memberToFollow);
            var memberToFollow2 = new Member();
            Context.Members.Add(memberToFollow2);
            Context.SaveChanges();
            var member = Context.Members.Include(x => x.FollowingMembers)
                .ThenInclude(x => x.Following)
                .First(x => x.Email == "admin@admin.com");
            member.Follow(memberToFollow);
            member.Follow(memberToFollow2);
            Context.SaveChanges();

            var response = await HttpClient.GetAsync($"/members/{member.Id}/followings");
            var memberResponse = await response.Content.ReadAsAsync<List<GetFollowingsFromMemberQuery.Response>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            memberResponse.Count.Should().Be(2);
            memberResponse.Count(x => x.Member.Id == memberToFollow.Id).Should().Be(1);
            memberResponse.Count(x => x.Member.Id == memberToFollow2.Id).Should().Be(1);
        }

    }
}
