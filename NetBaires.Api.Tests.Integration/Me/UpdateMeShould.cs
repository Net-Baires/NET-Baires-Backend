using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using FluentAssertions;
using static NetBaires.Api.Handlers.Events.GetMeHandler;
using static NetBaires.Api.Handlers.Events.UpdateMeHandler;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace NetBaires.Api.Tests.Integration
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
            var updateresponse = await (await HttpClient.PutAsync("/me", new FormUrlEncodedContent(paramsa))).Content.ReadAsAsync<UpdateMeResponse>();

            var meResponse = await (await HttpClient.GetAsync("/me")).Content.ReadAsAsync<GetMeResponse>();

            updateresponse.FirstName.Should().Be(meResponse.FirstName);
            updateresponse.Username.Should().Be(meResponse.Username);
            updateresponse.LastName.Should().Be(meResponse.LastName);
            updateresponse.Github.Should().Be(meResponse.Github);
        }
    }
}