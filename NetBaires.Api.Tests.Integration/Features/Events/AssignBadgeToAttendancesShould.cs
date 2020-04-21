using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Data;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class AssignBadgeToAttendancesShould : IntegrationTestsBase
    {
        public AssignBadgeToAttendancesShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Assign_Badge_To_All_Attended()
        {
            Event eventToAdd;
            Badge badge;
            FillData(out eventToAdd, out badge);
            eventToAdd.Complete();
            Context.SaveChanges();


            var response = await HttpClient.PostAsync($"/events/{eventToAdd.Id}/badges/{badge.Id}/Attendances", null);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();

            var badgeWithMembers = await Context.Badges.Include(x => x.Members)
                                                       .ThenInclude(x => x.Member)
                                                       .ThenInclude(x => x.Badges)
                                                       .FirstOrDefaultAsync(x => x.Id == badge.Id);

            badgeWithMembers.Members.Count.Should().Be(2);
            badgeWithMembers.Members[0].Member.Badges.Count.Should().Be(1);
            badgeWithMembers.Members[1].Member.Badges.Count.Should().Be(1);
            badgeWithMembers.Members[0].Member.Email.Should().Be("Test@test.com");
            badgeWithMembers.Members[1].Member.Email.Should().Be("Test2@test.com");
        }

        [Fact]
        public async Task Does_Not_Assign_Badge_Event_Is_Not_Done()
        {
            Event eventToAdd;
            Badge badge;
            FillData(out eventToAdd, out badge);
            Context.SaveChanges();
            var response = await HttpClient.PostAsync($"/events/{eventToAdd.Id}/badges/{badge.Id}/Attendances", null);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        private void FillData(out Event eventToAdd, out Badge badge)
        {
            eventToAdd = new Event();
            eventToAdd.AddAttendance(new Member
            {
                Email = "Test@test.com"
            }, AttendanceRegisterType.CurrentEvent).Attend();

            eventToAdd.AddAttendance(new Member
            {
                Email = "Test2@test.com"
            }, AttendanceRegisterType.CurrentEvent).Attend();

            eventToAdd.AddAttendance(new Member
            {
                Email = "Test3@test.com"
            }, AttendanceRegisterType.CurrentEvent).NoAttend();

            badge = new Badge();
            Context.Badges.Add(badge);
            Context.Members.Add(new Member { Email = "Test3@test.com" });

            Context.Events.Add(eventToAdd);
         
        }
    }
}
