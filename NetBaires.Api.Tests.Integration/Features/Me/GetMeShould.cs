using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.ViewModels;
using NetBaires.Data;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Me
{
    public class GetMeShould : IntegrationTestsBase
    {
        public GetMeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_Admin_Profile_Info()
        {
            var response = await HttpClient.GetAsync("/me");

            var meResponse = await response.Content.ReadAsAsync<MemberDetailViewModel>();

            response.StatusCode.Should().Be(200);
            meResponse.Email.Should().Be("admin@admin.com");
            meResponse.FirstName.Should().Be("Admin");
        }

        [Fact]
        public async Task Get_Followed()
        {
            var loggedUser = Context.Members.Include(x => x.FollowingMembers)
                  .ThenInclude(x => x.Following)
                  .First(x => x.Email == "admin@admin.com");

            var member1 = new Member {Id = 12};
            var member2 = new Member {Id = 13};
            var member3 = new Member {Id = 14};
            Context.Members.Add(member1);
            Context.Members.Add(member2);
            Context.Members.Add(member3);
            Context.SaveChanges();

            member1.Follow(loggedUser);
            Context.SaveChanges();
            member2.Follow(loggedUser);
            Context.SaveChanges();
            member3.Follow(loggedUser);
            Context.SaveChanges();

            var response = await HttpClient.GetAsync("/me");

            var meResponse = await response.Content.ReadAsAsync<MemberDetailViewModel>();

            meResponse.FollowedMembers.Count.Should().Be(3);
            meResponse.FollowedMembers.Contains(12);
            meResponse.FollowedMembers.Contains(13);
            meResponse.FollowedMembers.Contains(15);
        }
    }
}