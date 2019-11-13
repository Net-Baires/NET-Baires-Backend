using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Handlers.Events;
using NetBaires.Data;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace NetBaires.Api.Tests.Integration.Events
{
    public class GetEventsShould : IntegrationTestsBase
    {
        private Event _events;

        public GetEventsShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_204_Empty_Events()
        {
            var response = await HttpClient.GetAsync("/events");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Return_Done_events()
        {
            FillData();
            var response = await HttpClient.GetAsync("/events?done=true");
            var events = await response.Content.ReadAsAsync<List<GetEventsResponse>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            events.Count.Should().Be(2);
        }
        [Fact]
        public async Task Return_UnDone_events()
        {
            FillData();
            var response = await HttpClient.GetAsync("/events?done=false");
            var events = await response.Content.ReadAsAsync<List<GetEventsResponse>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            events.Count.Should().Be(4);
        }

        [Fact]
        public async Task Return_Events_Registered_ToBe_True()
        {
            FillData();
            var currentMember = Context.Members.First();
            _events.AddAttendance(currentMember);
            Context.SaveChanges();
            var response = await HttpClient.GetAsync("/events");
            var events = await response.Content.ReadAsAsync<List<GetEventsResponse>>();

            events.First(x => x.Id == _events.Id).Registered.Should().BeTrue(); ;

        }

        [Fact]
        public async Task Return_Live_events()
        {
            FillData();
            var response = await HttpClient.GetAsync("/events?live=true");
            var events = await response.Content.ReadAsAsync<List<GetEventsResponse>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            events.Count.Should().Be(2);
        }

        [Fact]
        public async Task Return_All_events()
        {
            FillData();
            var response = await HttpClient.GetAsync("/events");
            var events = await response.Content.ReadAsAsync<List<GetEventsResponse>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            events.Count.Should().Be(6);
        }

        private void FillData()
        {
            Context.Events.Add(new Event());
            var doneEvent = new Event();
            doneEvent.Complete();
            var doneEvent2 = new Event();
            doneEvent2.Complete();
            Context.Events.Add(doneEvent);
            Context.Events.Add(doneEvent2);
            Context.Events.Add(new Event());
            _events = new Event
            {
                Live = true
            };
            Context.Events.Add(_events);
            Context.Events.Add(new Event
            {
                Live = true
            });
            Context.SaveChanges();
        }
    }
}