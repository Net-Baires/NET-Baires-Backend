using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.ViewModels;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Me
{

    public class UpdateMeShould : IntegrationTestsBase
    {
        public UpdateMeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Update_Me_Profile()
        {
            var paramsa = new Dictionary<string, string>();
            paramsa.Add("Username", "NewUserName");
            paramsa.Add("FirstName", "NewFirstName");
            paramsa.Add("LastName", "NewLastName");
            paramsa.Add("Github", "NewGithub");
            paramsa.Add("Linkedin", "Linkedin");
            paramsa.Add("Twitter", "Twitter");
            paramsa.Add("Instagram", "Instagram");
            paramsa.Add("Biography", "Biography");
            paramsa.Add("WorkPosition", "WorkPosition");
            var updateresponse = await (await HttpClient.PutAsync("/me", new FormUrlEncodedContent(paramsa))).Content.ReadAsAsync<MemberDetailViewModel>();

            var meResponse = await (await HttpClient.GetAsync("/me")).Content.ReadAsAsync<MemberDetailViewModel>();

            updateresponse.FirstName.Should().Be(meResponse.FirstName);
            updateresponse.Username.Should().Be(meResponse.Username);
            updateresponse.LastName.Should().Be(meResponse.LastName);
            updateresponse.Github.Should().Be(meResponse.Github);
            updateresponse.Twitter.Should().Be(meResponse.Twitter);
            updateresponse.Linkedin.Should().Be(meResponse.Linkedin);
            updateresponse.Instagram.Should().Be(meResponse.Instagram);
            updateresponse.Biography.Should().Be(meResponse.Biography);
            updateresponse.WorkPosition.Should().Be(meResponse.WorkPosition);
        }
    }
}