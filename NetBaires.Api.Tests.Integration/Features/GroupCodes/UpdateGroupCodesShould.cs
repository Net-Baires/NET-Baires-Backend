using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Features.GroupsCodes.UpdateGroupCode;
using NetBaires.Data;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Newtonsoft.Json;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.GroupCodes
{
    public class UpdateGroupCodesShould : IntegrationTestsBase
    {
        private GroupCode _newGroupCode;

        public UpdateGroupCodesShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Update_GroupCode()
        {
            FillData();

            var update = new UpdateGroupCodeCommand
            {
                Detail = "New detail",
                Open = true
            };

            var response = await HttpClient.PutAsync($"/groupcodes/{_newGroupCode.Id}",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();
            var groupCodeCheck = Context.GroupCodes.First();
            groupCodeCheck.Detail.Should().Be(update.Detail);
            groupCodeCheck.Open.Should().Be(update.Open);
        }

        [Fact]
        public async Task Update_GroupCode_Close()
        {
            FillData();

            var update = new UpdateGroupCodeCommand
            {
                Open = true
            };

            var response = await HttpClient.PutAsync($"/groupcodes/{_newGroupCode.Id}",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();
            var groupCodeCheck = Context.GroupCodes.First();
            groupCodeCheck.Detail.Should().NotBeNullOrWhiteSpace();
            groupCodeCheck.Open.Should().Be(update.Open);
        }


        private void FillData()
        {
            _newGroupCode = new GroupCode("Detalle");
            Context.GroupCodes.Add(_newGroupCode);
            Context.SaveChanges();
        }
    }
}