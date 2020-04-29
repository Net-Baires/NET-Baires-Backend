using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Events.CreateGroupCode;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Newtonsoft.Json;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class CreateGroupCodeShould : IntegrationTestsBase
    {
        private Event _newEvent;

        public CreateGroupCodeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Create_New_Group_Code()
        {
            FillData();
            var newGroup = new CreateGroupCodeCommand
            {
                Detail = "Test Group Code"
            };

            var response = await HttpClient.PostAsync($"/events/{_newEvent.Id}/groupcodes",
                new StringContent(JsonConvert.SerializeObject(newGroup), Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseCreateGroupCode =await response.Content.ReadAsAsync<CreateGroupCodeCommand.Response>();

            responseCreateGroupCode.Code.Should().NotBeNullOrEmpty();
            RefreshContext();
            var sut = await Context.GroupCodes.FirstOrDefaultAsync();
            sut.Code.Should().Be(responseCreateGroupCode.Code);
            sut.Detail.Should().Be(newGroup.Detail);
            sut.Open.Should().BeFalse();
        }
        private void FillData()
        {
            _newEvent = new Event();

            Context.Events.Add(_newEvent);
            Context.SaveChanges();
        }
    }
}
