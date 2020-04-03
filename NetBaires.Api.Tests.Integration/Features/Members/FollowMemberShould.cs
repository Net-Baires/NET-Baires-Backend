using FluentAssertions;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Members.SearchMember;
using NetBaires.Api.Services.Meetup.Models;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Members
{
    public class UnFollowMemberShould : IntegrationTestsBase
    {
        public UnFollowMemberShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task UnFollow_New_Member()
        {
            var memberToFollow = new Data.Member();
            Context.Members.Add(memberToFollow);
            Context.SaveChanges();
            var member = Context.Members.Include(x => x.FollowingMembers)
                .ThenInclude(x => x.Followed)
                .First(x => x.Email == "admin@admin.com");
            member.Follow(memberToFollow);
            Context.SaveChanges();

            var response = await HttpClient.DeleteAsync($"/members/{memberToFollow.Id}/unFollow");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();


            member.FollowingMembers.Count.Should().Be(0);
        }

       
    }
}
