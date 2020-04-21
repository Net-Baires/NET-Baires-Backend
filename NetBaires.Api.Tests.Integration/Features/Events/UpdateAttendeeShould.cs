using FluentAssertions;
using NetBaires.Data;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NetBaires.Api.Features.Events.UpdateAttendee;
using NetBaires.Api.ViewModels;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class UpdateAttendeeShould : IntegrationTestsBase
    {
        private Event _newEvent;
        private Member _newMember;
        private Attendance _attendance;

        public UpdateAttendeeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Set_Attende_Like_Speaker()
        {
            FillData();

            var update = new UpdateAttendeeCommand
            {
                Speaker = true
            };

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}/Members/{_newMember.Id}/attende",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            var attendant = await response.Content.ReadAsAsync<AttendantViewModel>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            RefreshContext();

            var attendance = Context.Attendances.FirstOrDefault(x => x.MemberId == _newMember.Id);
            attendance.Speaker.Should().BeTrue();
            attendance.Attended.Should().BeTrue();
            attendance.DidNotAttend.Should().BeFalse();
        }

        [Fact]
        public async Task Set_Attende_Like_No_Speaker()
        {
            FillData();
            _attendance.SetSpeaker();
            Context.SaveChanges();
            var update = new UpdateAttendeeCommand
            {
                Speaker = false
            };

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}/Members/{_newMember.Id}/attende",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            var attendant = await response.Content.ReadAsAsync<AttendantViewModel>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            RefreshContext();

            var attendance = Context.Attendances.FirstOrDefault(x => x.MemberId == _newMember.Id);
            attendance.Speaker.Should().BeFalse();
        }

        [Fact]
        public async Task Set_Attende_Like_Organizer()
        {
            FillData();

            var update = new UpdateAttendeeCommand
            {
                Organizer = true
            };

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}/Members/{_newMember.Id}/attende",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            var attendant = await response.Content.ReadAsAsync<AttendantViewModel>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            RefreshContext();

            var attendance = Context.Attendances.FirstOrDefault(x => x.MemberId == _newMember.Id);
            attendance.Speaker.Should().BeFalse();
            attendance.Organizer.Should().BeTrue();
            attendance.Attended.Should().BeTrue();
            attendance.DidNotAttend.Should().BeFalse();
        }

        [Fact]
        public async Task Set_Attende_Like_No_Organizer()
        {
            FillData();
            _newEvent.Attended(_newMember, AttendanceRegisterType.CurrentEvent);
            Context.SaveChanges();
            var update = new UpdateAttendeeCommand
            {
                Organizer = false
            };

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}/Members/{_newMember.Id}/attende",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            var attendant = await response.Content.ReadAsAsync<AttendantViewModel>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            RefreshContext();

            var attendance = Context.Attendances.FirstOrDefault(x => x.MemberId == _newMember.Id);
            attendance.Speaker.Should().BeFalse();
            attendance.Organizer.Should().BeFalse();
            attendance.Attended.Should().BeTrue();
            attendance.DidNotAttend.Should().BeFalse();
        }
        [Fact]
        public async Task Set_Attended()
        {
            FillData();
            Context.SaveChanges();
            var update = new UpdateAttendeeCommand
            {
                Attended = true
            };

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}/Members/{_newMember.Id}/attende",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            var attendant = await response.Content.ReadAsAsync<AttendantViewModel>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            RefreshContext();

            var attendance = Context.Attendances.FirstOrDefault(x => x.MemberId == _newMember.Id);
            attendance.Speaker.Should().BeFalse();
            attendance.Attended.Should().BeTrue();
            attendance.DidNotAttend.Should().BeFalse();
        }
        [Fact]
        public async Task Set_Did_Not_Attended()
        {
            FillData();
            Context.SaveChanges();
            var update = new UpdateAttendeeCommand
            {
                Attended = false
            };

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}/Members/{_newMember.Id}/attende",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            var attendant = await response.Content.ReadAsAsync<AttendantViewModel>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            RefreshContext();

            var attendance = Context.Attendances.FirstOrDefault(x => x.MemberId == _newMember.Id);
            attendance.Speaker.Should().BeFalse();
            attendance.Attended.Should().BeFalse();
            attendance.DidNotAttend.Should().BeTrue();
        }

        [Fact]
        public async Task Set_NotifiedAbsence()
        {
            FillData();
            Context.SaveChanges();
            var update = new UpdateAttendeeCommand
            {
                NotifiedAbsence = true
            };

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}/Members/{_newMember.Id}/attende",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            var attendant = await response.Content.ReadAsAsync<AttendantViewModel>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            RefreshContext();

            var attendance = Context.Attendances.FirstOrDefault(x => x.MemberId == _newMember.Id);
            attendance.Speaker.Should().BeFalse();
            attendance.Attended.Should().BeFalse();
            attendance.DidNotAttend.Should().BeTrue();
            attendance.NotifiedAbsence.Should().BeTrue();
        }
        private void FillData()
        {
            _newEvent = new Event();
            _newMember = new Member
            {
                Email = "test@test.com",
                Biography = "Biography",
                FirstName = "FirstName",
                Github = "Github",
                Instagram = "Instagram",
                LastName = "LastName",
                Linkedin = "Linkedin"
            };
            _attendance = _newEvent.AddAttendance(_newMember, AttendanceRegisterType.CurrentEvent);
            Context.Events.Add(_newEvent);
            Context.SaveChanges();
        }
    }
}