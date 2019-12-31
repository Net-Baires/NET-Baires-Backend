using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NetBaires.Api.Models.ServicesResponse.Attendance;
using NetBaires.Api.ViewModels;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class PutCheckAttendanceGeneralShould : IntegrationTestsBase
    {
        public PutCheckAttendanceGeneralShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Report_Attendance_Of_Member_In_Token()
        {
            var newEvent = new Event
            {
                Title = "Event Test",
                Description = "Event Description Test",
                GeneralAttended = true
            };
            var newMember = new Member
            {
                Email = "test@test.com",
                Role = UserRole.Member
            };
            Context.Events.Add(newEvent);
            Context.Members.Add(newMember);
            Context.SaveChanges();
            await AuthenticateAsync(newMember.Email);
            var eventToCheck = await (await HttpClient.GetAsync($"/events/{newEvent.Id}/Attendance"))
                        .Content.ReadAsAsync<EventToReportAttendanceViewModel>();

            var response = await HttpClient.PutAsync($"/events/Attendances/general/{eventToCheck.Token}", null);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<CheckAttendanceGeneralResponse>()).EventId.Should().Be(newEvent.Id);

            var eventToTest = await Context.Events.Include(x => x.Attendees).FirstAsync();
            eventToTest.Attendees.Count.Should().Be(1);
            eventToTest.Attendees.First().MemberId.Should().Be(newMember.Id);
            eventToTest.Attendees.First().Attended.Should().BeTrue();
            eventToTest.Attendees.First().DidNotAttend.Should().BeFalse();
            eventToTest.Attendees.First().DoNotKnow.Should().BeFalse();
            eventToTest.Attendees.First().NotifiedAbsence.Should().BeFalse();
        }
        [Fact]
        public async Task Does_Not_Report_Attendance_Event_Has_not_GeneralAttended()
        {
            var newEvent = new Event
            {
                Title = "Event Test",
                Description = "Event Description Test",
                GeneralAttended = false
            };
            var newMember = new Member
            {
                Email = "test@test.com",
                Role = UserRole.Member
            };
            Context.Events.Add(newEvent);
            Context.Members.Add(newMember);
            Context.SaveChanges();
            await AuthenticateAsync(newMember.Email);
            var eventToCheck = await (await HttpClient.GetAsync($"/events/{newEvent.Id}/Attendance"))
                        .Content.ReadAsAsync<EventToReportAttendanceViewModel>();

            var response = await HttpClient.PutAsync($"/events/Attendances/general/{eventToCheck.Token}", null);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }
    }
}