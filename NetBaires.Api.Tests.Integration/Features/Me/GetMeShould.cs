using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Features.Members.ViewModels;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Me
{
    public class GetMeShould : IntegrationTestsBase
    {
        public GetMeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_Admin_Profile_Info()
        {
            var response = await HttpClient.GetAsync("/me");

            var meResponse = await response.Content.ReadAsAsync<MemberDetailViewModel>();

            response.StatusCode.Should().Be(200);
            meResponse.Email.Should().Be("admin@admin.com");
            meResponse.FirstName.Should().Be("Admin");
        }
    }
}