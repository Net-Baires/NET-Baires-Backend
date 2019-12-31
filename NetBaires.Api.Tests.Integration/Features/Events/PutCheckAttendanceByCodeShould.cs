using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NetBaires.Api.Models.ServicesResponse.Attendance;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class PutCheckAttendanceByCodeShould : IntegrationTestsBase
    {
        private Member _newMember;
        private Event _newEvent;

        public PutCheckAttendanceByCodeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Report_Attendance_Of_Member()
        {
            FillData();
            await AuthenticateAsync(_newMember.Email);

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}/Attendances/general/{_newEvent.GeneralAttendedCode}", null);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<CheckAttendanceGeneralResponse>()).EventId.Should().Be(_newEvent.Id);
            RefreshContext();
            var eventToTest = await Context.Events.Include(x => x.Attendees).FirstAsync();
            eventToTest.Attendees.Count.Should().Be(1);
            eventToTest.Attendees.First().MemberId.Should().Be(_newMember.Id);
            eventToTest.Attendees.First().Attended.Should().BeTrue();
        }


        [Fact]
        public async Task Not_Report_Attendance_Of_Member_Wrong_Code()
        {
            FillData();

            await AuthenticateAsync(_newMember.Email);

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}/Attendances/general/12312", null);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        private void FillData()
        {
            _newEvent = new Event
            {
                Title = "Event Test",
                Description = "Event Description Test"
            };
            _newEvent.EnableGeneralAttendace();
            _newMember = new Member
            {
                Email = "test@test.com",
                Role = UserRole.Member
            };
            Context.Events.Add(_newEvent);
            Context.Members.Add(_newMember);
            Context.SaveChanges();
        }

    }
}