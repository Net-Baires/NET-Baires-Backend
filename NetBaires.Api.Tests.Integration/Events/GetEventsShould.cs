using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace NetBaires.Api.Tests.Integration.Events
{
    public class GetEventsShould : IntegrationTestsBase
    {
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

    }
}