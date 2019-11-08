using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Events
{
    public class SyncWithExternalEventsShould : IntegrationTestsBase
    {
        public SyncWithExternalEventsShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_204_Empty_Events()
        {
            var defaultPage = await HttpClient.PutAsync("/events/sync", null);

            defaultPage.StatusCode.Should().Be(HttpStatusCode.NoContent);

        }

    }
}