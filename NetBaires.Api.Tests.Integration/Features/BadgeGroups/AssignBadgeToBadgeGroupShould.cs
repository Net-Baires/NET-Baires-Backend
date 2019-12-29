using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.BadgeGroups
{
    public class AssignBadgeToBadgeGroupShould : IntegrationTestsBase
    {
        public AssignBadgeToBadgeGroupShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }


        [Fact]
        public async Task Assign_Badge_To_BadgeGroup()
        {
            var badgeGroup = new BadgeGroup();
            var badge = new Badge();
            Context.BadgeGroups.Add(badgeGroup);
            Context.Badges.Add(badge);

            Context.SaveChanges();
            var response = await HttpClient.PostAsync($"badgeGroups/{badgeGroup.Id}/badges/{badge.Id}", null);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();
             badgeGroup = Context.BadgeGroups.Include(x => x.Badges).FirstOrDefault();
            badgeGroup.Badges.Count().Should().Be(1);
            badgeGroup.Badges.First().Id.Should().Be(badge.Id);
        }
    }
}