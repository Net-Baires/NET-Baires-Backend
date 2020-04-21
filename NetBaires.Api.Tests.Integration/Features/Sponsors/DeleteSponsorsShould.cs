using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Data;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Sponsors
{
    public class DeleteSponsorsShould : IntegrationTestsBase
    {

        public DeleteSponsorsShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }
        [Fact]
        public async Task Delete_Sponsor()
        {
            var newSponsor = new Sponsor();
            Context.Sponsors.Add(newSponsor);
            Context.SaveChanges();
            var response = await HttpClient.DeleteAsync($"/sponsors/{newSponsor.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();
            Context.Sponsors.Count().Should().Be(0);
        }
    }
}