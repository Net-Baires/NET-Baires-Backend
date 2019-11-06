using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure;
using System.Net.Http;
using FluentAssertions;
using System.Net;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace NetBaires.Api.Tests.Integration
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
            var defaultPage = await HttpClient.GetAsync("/events");

            defaultPage.StatusCode.Should().Be(HttpStatusCode.NoContent);

        }

    }
}