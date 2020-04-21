using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Features.Events.GetSpeakersInEvent;
using NetBaires.Data;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;
namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class GetSpeakersInEventShould : IntegrationTestsBase
    {
        private Event _event;

        public GetSpeakersInEventShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }

        //[Fact]
        //public async Task Return_204_Empty_Events()
        //{
        //    FillData();
        //    var response = await HttpClient.GetAsync($"/events/{_event.Id}/speakers");

        //    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        //}

        [Fact]
        public async Task Return_All_Speakers()
        {
            FillData();
            _event.AddSpeaker(new Member { Id = 23});
            _event.AddSpeaker(new Member { Id = 24 });
            _event.AddSpeaker(new Member { Id = 25 });

            _event.AddAttendance(new Member(), AttendanceRegisterType.CurrentEvent);
            _event.AddAttendance(new Member(), AttendanceRegisterType.CurrentEvent);
            Context.SaveChanges();
            var response = await HttpClient.GetAsync($"/events/{_event.Id}/speakers");
            var events = await response.Content.ReadAsAsync<List<GetSpeakersInEventInEventResponse>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            events.Count.Should().Be(3);
        }

        private void FillData()
        {
            _event = new Event
            {
                Live = true
            };
            Context.Events.Add(_event);
            Context.SaveChanges();
        }
    }
}