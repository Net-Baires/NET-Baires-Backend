using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.ViewModels;
using NetBaires.Data;
using NetBaires.Host;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class GetEventsShould : IntegrationTestsBase
    {
        private Event _event;

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
            var events = await response.Content.ReadAsAsync<List<EventDetailViewModel>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            events.Count.Should().Be(2);
        }
        [Fact]
        public async Task Return_UnDone_events()
        {
            FillData();
            var response = await HttpClient.GetAsync("/events?done=false");
            var events = await response.Content.ReadAsAsync<List<EventDetailViewModel>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            events.Count.Should().Be(4);
        }

        [Fact]
        public async Task Return_Events_Registered_ToBe_True()
        {
            FillData();
            var currentMember = Context.Members.First();
            _event.AddAttendance(currentMember);
            Context.SaveChanges();
            var response = await HttpClient.GetAsync("/events");
            var events = await response.Content.ReadAsAsync<List<EventDetailViewModel>>();

            events.First(x => x.Id == _event.Id).IsUserRegistered.Should().BeTrue(); ;

        }

        [Fact]
        public async Task Return_Only_One_Event()
        {
            FillData();
            var currentMember = Context.Members.First();
            _event.AddAttendance(currentMember);
            Context.SaveChanges();
            var response = await HttpClient.GetAsync($"/events/{_event.Id}");
            var eventResult = await response.Content.ReadAsAsync<EventDetailViewModel>();
            eventResult.Id.Should().Be(_event.Id);
        }

        [Fact]
        public async Task Return_Live_events()
        {
            FillData();
            var response = await HttpClient.GetAsync("/events?live=true");
            var events = await response.Content.ReadAsAsync<List<EventDetailViewModel>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            events.Count.Should().Be(2);
        }

        [Fact]
        public async Task Return_All_events()
        {
            FillData();
            var response = await HttpClient.GetAsync("/events");
            var events = await response.Content.ReadAsAsync<List<EventDetailViewModel>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            events.Count.Should().Be(6);
        }
        [Fact]
        public async Task Return_Events_To_Sync()
        {

            var newEvent = new Event
            {
                Attendees = new List<Attendance> {
                            new Attendance{
                                Attended=false,
                                DidNotAttend=true
                            },
                            new Attendance{
                                Attended=false,
                                DidNotAttend=true
                            },
                            new Attendance{
                                Attended=true,
                                DidNotAttend=false
                            }
                           }
            };
            Context.Events.Add(newEvent);
            Context.SaveChanges();
            var response = await HttpClient.GetAsync("/events?done=false");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var events = await response.Content.ReadAsAsync<List<EventDetailViewModel>>();
            events.Count.Should().Be(1);
            events.First().Attended.Should().Be(1);
            events.First().DidNotAttend.Should().Be(2);
            events.First().Registered.Should().Be(3);

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
            _event = new Event
            {
                Live = true
            };
            Context.Events.Add(_event);
            Context.Events.Add(new Event
            {
                Live = true
            });
            Context.SaveChanges();
        }
    }
}