using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Data;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Badges
{
    public class DeleteBadgeShould : IntegrationTestsBase
    {
        public DeleteBadgeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Delete_Badge()
        {

            var newBadge = new Data.Badge();
            Context.Badges.Add(newBadge);
            await Context.SaveChangesAsync();

            var response = await HttpClient.DeleteAsync($"/badges/{newBadge.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Badges.FirstOrDefault(x => x.Id == newBadge.Id).Should().BeNull();
        }

        [Fact]
        public async Task Not_Delete_Badge_Has_Members()
        {
            var newBadge = new Data.Badge();
            newBadge.Members = new List<BadgeMember>();
            newBadge.Members.Add(new BadgeMember
            {
                Member = new Data.Member
                {
                    Email = "Newuser@user.com"
                }
            });
            Context.Badges.Add(newBadge);
            await Context.SaveChangesAsync();

            var response = await HttpClient.DeleteAsync($"/badges/{newBadge.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);

            Context.Badges.FirstOrDefault(x => x.Id == newBadge.Id).Should().NotBeNull();
        }
        [Fact]
        public async Task Not_Found()
        {
            var response = await HttpClient.DeleteAsync($"/badges/1234");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}