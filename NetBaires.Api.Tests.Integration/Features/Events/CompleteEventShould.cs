using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Internal;
using NetBaires.Api.Features.Events.CompleteEvent;
using NetBaires.Data;
using NetBaires.Data.DomainEvents;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Newtonsoft.Json;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class CompleteEventShould : IntegrationTestsBase
    {
        public CompleteEventShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Update_event_Done_All_Not_Attended_Mark_DidNotAttended()
        {
            Event eventToAdd = FillData();
            var response = await HttpClient.PutAsync($"/events/{eventToAdd.Id}/done",
                new StringContent(JsonConvert.SerializeObject(new CompleteEventCommand()), Encoding.UTF8,
                    "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();
            eventToAdd.Done.Should().BeTrue();

            eventToAdd.Attendees[0].Attended.Should().BeTrue();
            eventToAdd.Attendees[1].Attended.Should().BeFalse();
            eventToAdd.Attendees[1].DidNotAttend.Should().BeTrue();
            eventToAdd.Attendees[2].Attended.Should().BeFalse();
            eventToAdd.Attendees[2].DidNotAttend.Should().BeTrue();
        }

        [Fact]
        public async Task Complete_Event_Send_Material_To_Attendees()
        {
            Event eventToAdd = FillData();
            var memberAttended = new Member
            {
                Email = "Email@test.com"
            };
            eventToAdd.AddAttendance(memberAttended, AttendanceRegisterType.CurrentEvent).Attend();
            Context.SaveChanges();
            var command = new CompleteEventCommand
            {
                SendMaterialToAttendees = true,
                ThanksAttendees = true
            };
            await HttpClient.PutAsync($"/events/{eventToAdd.Id}/done",
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));


            var list = QueueServices.GetMessages<ToThankAttended>();
            list.Count.Should().Be(2);
            list.ToList().Any(x => x.MemberId == memberAttended.Id
                                   &&
                                   x.EventId == eventToAdd.Id
                                   &&
                                   x.SendMaterialToAttendee).Should().BeTrue();
        }

        [Fact]
        public async Task Complete_Event_Does_Not_Send_Material_To_Attendees()
        {
            Event eventToAdd = FillData();

            var command = new CompleteEventCommand
            {
                ThanksAttendees = true
            };
            var response = await HttpClient.PutAsync($"/events/{eventToAdd.Id}/done",
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));


            var list = QueueServices.GetMessages<ToThankAttended>();
            list.Count.Should().Be(1);
            list.ToList().Any(x => x.EventId == eventToAdd.Id
                                   &&
                                   !x.SendMaterialToAttendee).Should().BeTrue();
        }

        [Fact]
        public async Task Complete_Event_Thanks_Speakers()
        {
            Event eventToAdd = FillData();

            var command = new CompleteEventCommand
            {
                ThanksSpeakers = true
            };
            var response = await HttpClient.PutAsync($"/events/{eventToAdd.Id}/done",
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));


            var list = QueueServices.GetMessages<ToThankSpeakers>();
            list.Count.Should().Be(1);
            list.ToList().Any(x => x.EventId == eventToAdd.Id).Should().BeTrue();
        }

        [Fact]
        public async Task Complete_Event_Thanks_Sponsors()
        {
            Event eventToAdd = FillData();

            var command = new CompleteEventCommand
            {
                ThanksSponsors = true
            };
            var response = await HttpClient.PutAsync($"/events/{eventToAdd.Id}/done",
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));


            var list = QueueServices.GetMessages<ToThankSponsors>();
            list.Count.Should().Be(1);
            list.ToList().Any(x => x.EventId == eventToAdd.Id).Should().BeTrue();
        }



        private Event FillData()
        {
            Event eventToAdd = new Event();
            eventToAdd.AddAttendance(new Member
            {
                Email = "Test@test.com"
            }, AttendanceRegisterType.CurrentEvent).Attend();

            eventToAdd.AddAttendance(new Member
            {
                Email = "Test2@test.com"
            }, AttendanceRegisterType.CurrentEvent);

            eventToAdd.AddAttendance(new Member
            {
                Email = "Test3@test.com"
            }, AttendanceRegisterType.CurrentEvent).SetDoNotKnow();

            Context.Members.Add(new Member { Email = "Test3@test.com" });

            Context.Events.Add(eventToAdd);
            Context.SaveChanges();

            return eventToAdd;
        }

        public override void Dispose()
        {
            QueueServices.Clear<ToThankAttended>();
            QueueServices.Clear<ToThankSpeakers>();
            QueueServices.Clear<ToThankSponsors>();

        }
    }
}
