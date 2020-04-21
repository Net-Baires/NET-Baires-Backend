using FluentAssertions;
using NetBaires.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NetBaires.Api.Features.BadgeGroups.ViewModels;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.BadgeGroups
{
    public class GetBadgeGroupsShould : IntegrationTestsBase
    {
        public GetBadgeGroupsShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_BadgeGroups()
        {
            Context.BadgeGroups.Add(new BadgeGroup
            {
                Badges = new List<Badge> {
                    new Badge(),
                    new Badge(),
                    new Badge()
                }
            });
            Context.BadgeGroups.Add(new BadgeGroup());

            await Context.SaveChangesAsync();
            var response = await HttpClient.GetAsync("/badgegroups");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var badgeGroups = await response.Content.ReadAsAsync<List<BadgeGroupDetailViewModel>>();
            badgeGroups.Count.Should().Be(2);
            badgeGroups.First().Badges.Should().Be(3);
        }

    }
}
