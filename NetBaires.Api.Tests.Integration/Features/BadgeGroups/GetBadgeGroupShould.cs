using FluentAssertions;
using NetBaires.Data;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NetBaires.Api.Features.BadgeGroups.ViewModels;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.BadgeGroups
{
    public class GetBadgeGroupShould : IntegrationTestsBase
    {
        public GetBadgeGroupShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_BadgeGroup()
        {
            var group = new Data.BadgeGroup
            {
                Badges = new List<Badge> {
                    new Badge(),
                    new Badge(),
                    new Badge()
                }
            };
            Context.BadgeGroups.Add(group);
            Context.BadgeGroups.Add(new BadgeGroup());

            await Context.SaveChangesAsync();

            var response = await HttpClient.GetAsync($"/badgegroups/{group.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var badgeGroups = await response.Content.ReadAsAsync<BadgeGroupDetailViewModel>();
            badgeGroups.Badges.Should().Be(3);
        }

    }
}
