using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Features.Events.ViewModels;
using NetBaires.Api.Handlers.Events;
using NetBaires.Data;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Events
{
    public class GetAttendeesShould : IntegrationTestsBase
    {
        public GetAttendeesShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Return_All_Attendees()
        {
            var newEvent = new Event();
            newEvent.AddAttendance(new Member());
            newEvent.AddAttendance(new Member());
            newEvent.AddAttendance(new Member());
            Context.Events.Add(newEvent);
            Context.SaveChanges();
            var response = await HttpClient.GetAsync($"/events/{newEvent.Id}/attendees");
            var events = await response.Content.ReadAsAsync<List<AttendantViewModel>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            events.Count.Should().Be(3);
        }
    }
}