using System.Linq;
using FluentAssertions;
using NetBaires.Data;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.ViewModels;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Members
{
    public class GetMemberDetailShould : IntegrationTestsBase
    {
        public GetMemberDetailShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_Member_Detail()
        {
            var member = new Member
            {
                Email = "Test@test.com",
                Github = "Github Test"
            };

            await Context.Members.AddAsync(member);
            await Context.SaveChangesAsync();
            var newEventOne = new Event();
            var newEventTwo = new Event();
            var newEventThree = new Event();
            await Context.Events.AddAsync(newEventOne);
            await Context.Events.AddAsync(newEventTwo);
            await Context.Events.AddAsync(newEventThree);
            await Context.SaveChangesAsync();
            newEventOne.AddAttendance(member, AttendanceRegisterType.CurrentEvent).Attend();
            newEventTwo.AddAttendance(member, AttendanceRegisterType.CurrentEvent).Attend();
            newEventThree.AddAttendance(member, AttendanceRegisterType.CurrentEvent).NoAttend();
            await Context.SaveChangesAsync();

            var response = await HttpClient.GetAsync($"/members/{member.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var memberResponse = await response.Content.ReadAsAsync<MemberDetailViewModel>();
            memberResponse.Should().NotBeNull();
            memberResponse.Email.Should().Be(member.Email);
            memberResponse.Github.Should().Be(member.Github);
            RefreshContext();
            var memberToCheck = await Context.Members.Include(x => x.Events).Where(x => x.Id == member.Id)
                .FirstOrDefaultAsync();
            memberResponse.AverageAttendance.Should().Be((memberToCheck.Events.Count(e => e.Attended) * 100) / memberToCheck.Events.Count);
        }

        [Fact]
        public async Task Return_True_Current_User_Follow_Member()
        {
            var memberToFollow = new Member();
            Context.Members.Add(memberToFollow);
            Context.SaveChanges();
            RefreshContext();
            var memberLogged = Context.Members.Include(x => x.FollowingMembers)
                .ThenInclude(x => x.Following)
                .First(x => x.Email == "admin@admin.com");
            memberLogged.Follow(memberToFollow);
            Context.SaveChanges();

            var response = await HttpClient.GetAsync($"/members/{memberToFollow.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var memberResponse = await response.Content.ReadAsAsync<MemberDetailViewModel>();
            memberResponse.Following.Should().BeTrue();
        }

        [Fact]
        public async Task Return_NoContent()
        {
            var response = await HttpClient.GetAsync($"/members/13123123");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
