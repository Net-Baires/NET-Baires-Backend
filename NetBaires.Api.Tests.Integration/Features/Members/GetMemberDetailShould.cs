using FluentAssertions;
using NetBaires.Api.Features.Members.ViewModels;
using NetBaires.Data;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.BadgeGroups
{
    public class GetMemberDetailShould : IntegrationTestsBase
    {
        public GetMemberDetailShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_Member_Detail()
        {
            var member = new Member
            {
                Email = "Test@test.com",
                Github="Github Test"
            };
            await Context.Members.AddAsync(member);
            await Context.SaveChangesAsync();

            var response = await HttpClient.GetAsync($"/members/{member.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var memberResponse = await response.Content.ReadAsAsync<MemberDetailViewModel>();
            memberResponse.Should().NotBeNull();
            memberResponse.Email.Should().Be(member.Email);
            memberResponse.Github.Should().Be(member.Github);
        }

        [Fact]
        public async Task Return_NoContent()
        {
            var response = await HttpClient.GetAsync($"/members/13123123");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
