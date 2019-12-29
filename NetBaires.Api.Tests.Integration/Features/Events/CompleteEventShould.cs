using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Data;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class CompleteEventShould : IntegrationTestsBase
    {
        public CompleteEventShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Update_event_Done_All_Not_Atteded_Mark_DidNotAttended()
        {
            Event eventToAdd = FillData();
            var response = await HttpClient.PutAsync($"/events/{eventToAdd.Id}/done", null);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();
            eventToAdd.Done.Should().BeTrue();

            eventToAdd.Attendees[0].Attended.Should().BeTrue();
            eventToAdd.Attendees[1].Attended.Should().BeFalse();
            eventToAdd.Attendees[1].DidNotAttend.Should().BeTrue();
            eventToAdd.Attendees[2].Attended.Should().BeFalse();
            eventToAdd.Attendees[2].DidNotAttend.Should().BeTrue();
        }
        private Event FillData()
        {
            Event eventToAdd = new Event();
            eventToAdd.AddAttendance(new Member
            {
                Email = "Test@test.com"
            }).Attend();

            eventToAdd.AddAttendance(new Member
            {
                Email = "Test2@test.com"
            });

            eventToAdd.AddAttendance(new Member
            {
                Email = "Test3@test.com"
            }).SetDoNotKnow();

            Context.Members.Add(new Member { Email = "Test3@test.com" });

            Context.Events.Add(eventToAdd);
            Context.SaveChanges();

            return eventToAdd;
        }
    }
}
