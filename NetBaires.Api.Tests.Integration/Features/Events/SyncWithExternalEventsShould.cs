using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Data;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class SyncWithExternalEventsShould : IntegrationTestsBase
    {
        public SyncWithExternalEventsShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Add_One_EventBrite_Event()
        {
            var defaultPage = await HttpClient.PutAsync("/events/sync", null);

            defaultPage.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var eventBriteEvents = Context.Events.Where(x => x.Platform == EventPlatform.EventBrite).ToList();
            eventBriteEvents.Count.Should().Be(1);
        }

        [Fact]
        public async Task Add_Two_Meetup_Event()
        {
            var defaultPage = await HttpClient.PutAsync("/events/sync", null);

            defaultPage.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var eventBriteEvents = Context.Events.Where(x => x.Platform == EventPlatform.Meetup).ToList();
            eventBriteEvents.Count.Should().Be(2);
        }
        [Fact]
        public async Task Add_One_Meetup_Event()
        {
            Context.Events.Add(new Event
            {
                EventId = "1234"
            });
            Context.SaveChanges();
            var defaultPage = await HttpClient.PutAsync("/events/sync", null);

            defaultPage.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var eventBriteEvents = Context.Events.Where(x => x.Platform == EventPlatform.Meetup).ToList();
            eventBriteEvents.Count.Should().Be(2);
        }


        
    }
}