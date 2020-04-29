using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Internal;
using NetBaires.Api.ViewModels;
using NetBaires.Data;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Sponsors
{
    public class GetSponsorsShould : IntegrationTestsBase
    {
        private readonly Sponsor _sponsor;

        public GetSponsorsShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
            _sponsor = new Sponsor
            {
                Name = "Sponsor Name",
                Description = "Sponsor Description",
                SiteUrl = "Sponsor SiteUrl",
                Email = "email@email"
            };
        }
        [Fact]
        public async Task Return_All_Sponsors()
        {
            Context.Sponsors.Add(new Sponsor());
            Context.Sponsors.Add(new Sponsor());
            Context.Sponsors.Add(new Sponsor());
            Context.Sponsors.Add(_sponsor);
            Context.SaveChanges();
            var response = await HttpClient.GetAsync("/sponsors");
            var sponsors = await response.Content.ReadAsAsync<List<SponsorDetailViewModel>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            sponsors.Count.Should().Be(4);
            sponsors.Any(x => x.Email == _sponsor.Email).Should().BeTrue();
            sponsors.Any(x => x.SiteUrl == _sponsor.SiteUrl).Should().BeTrue();
            sponsors.Any(x => x.Description == _sponsor.Description).Should().BeTrue();
            sponsors.Any(x => x.Name == _sponsor.Name).Should().BeTrue();
        }

        [Fact]
        public async Task Return_One_Sponsors()
        {
            Context.Sponsors.Add(new Sponsor());
            Context.Sponsors.Add(_sponsor);
            Context.SaveChanges();
            var response = await HttpClient.GetAsync($"/sponsors/{_sponsor.Id}");
            var sponsorReturn = await response.Content.ReadAsAsync<SponsorDetailViewModel>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            sponsorReturn.Name.Should().Be(_sponsor.Name);
            sponsorReturn.Description.Should().Be(_sponsor.Description);
            sponsorReturn.Email.Should().Be(_sponsor.Email);
            sponsorReturn.SiteUrl.Should().Be(_sponsor.SiteUrl);
        }
    }
}