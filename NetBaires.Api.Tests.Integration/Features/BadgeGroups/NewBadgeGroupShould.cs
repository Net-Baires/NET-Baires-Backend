using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Features.BadgeGroups.NewBadgeGroups;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.BadgeGroups
{
    public class NewBadgeGroupShould : IntegrationTestsBase
    {
        public NewBadgeGroupShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }


        [Fact]
        public async Task Add_New_BadgeGroup()
        {
            var response = await HttpClient.PostAsJsonAsync("/badgegroups", new NewBadgeGroupCommand("Name","Description"));
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var badgeGroup = Context.BadgeGroups.FirstOrDefault();
            badgeGroup.Name.Should().Be("Name");
            badgeGroup.Description.Should().Be("Description");
        }
      
    }
}