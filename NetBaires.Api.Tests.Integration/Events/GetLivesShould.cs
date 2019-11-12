using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Handlers.Events;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Events
{
    public class GetLivesShould : IntegrationTestsBase
    {
        public GetLivesShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_Lives_Events()
        {
            Context.Events.Add(new Data.Event
            {
                Live = true,
            });
            Context.Events.Add(new Data.Event
            {
                Live = true,
            });
            Context.Events.Add(new Data.Event
            {
                Live = false,
            });
            Context.SaveChanges();
            var response = await HttpClient.GetAsync("/events/lives");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var eventsLive = await response.Content.ReadAsAsync<List<GetLivesResponse>>();
            eventsLive.Count.Should().Be(2);
        }
    }
}