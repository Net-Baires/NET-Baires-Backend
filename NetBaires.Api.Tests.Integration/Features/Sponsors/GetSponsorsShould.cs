using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.ViewModels;
using NetBaires.Data;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Sponsors
{
    public class GetSponsorsShould : IntegrationTestsBase
    {

        public GetSponsorsShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }
        [Fact]
        public async Task Return_All_Sponsors()
        {
            Context.Sponsors.Add(new Sponsor());
            Context.Sponsors.Add(new Sponsor());
            Context.Sponsors.Add(new Sponsor());
            Context.SaveChanges();
            var response = await HttpClient.GetAsync("/sponsors");
            var events = await response.Content.ReadAsAsync<List<SponsorDetailViewModel>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            events.Count.Should().Be(3);
        }

        [Fact]
        public async Task Return_One_Sponsors()
        {
            var sponsor = new Sponsor
            {
                Name = "Sponsor Name",
                Description = "Sponsor Description",
                SiteUrl = "Sponsor SiteUrl"
            };
            Context.Sponsors.Add(new Sponsor());
            Context.Sponsors.Add(sponsor);
            Context.SaveChanges();
            var response = await HttpClient.GetAsync($"/sponsors/{sponsor.Id}");
            var sponsorReturn = await response.Content.ReadAsAsync<SponsorDetailViewModel>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            sponsorReturn.Name.Should().Be(sponsor.Name);
            sponsorReturn.Description.Should().Be(sponsor.Description);
            sponsorReturn.SiteUrl.Should().Be(sponsor.SiteUrl);
        }
    }
}