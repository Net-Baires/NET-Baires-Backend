using FluentAssertions;
using NetBaires.Data;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NetBaires.Api.Features.Members.AddMember;
using NetBaires.Api.Features.Members.SearchMember;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Members
{
    public class UpdateInformationShould : IntegrationTestsBase
    {
        public UpdateInformationShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Update_Information()
        {
            var newMember = new UpdateInformationCommand
            {
                PushNotificationId = "PushNotificationId1"
            };

            var response = await HttpClient.PutAsync($"/members/information",
                new StringContent(JsonConvert.SerializeObject(newMember), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();

            var member = Context.Members.First(x => x.Email == "admin@admin.com");
            member.PushNotificationId.Should().Be(newMember.PushNotificationId);
        }
    }
}
