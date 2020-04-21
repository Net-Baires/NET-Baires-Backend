using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Features.Events.AddCurrentUserToGroupCode;
using NetBaires.Data;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class AddCurrentUserToGroupCodeShould : IntegrationTestsBase
    {
        private GroupCode _groupCode;
        private Event _eventToAdd;
        private Member _member;

        public AddCurrentUserToGroupCodeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            FillData().GetAwaiter().GetResult();
            AuthenticateAsync(_member.Email).GetAwaiter().GetResult();

        }

        [Fact]
        public async Task Not_Found_Invalid_Code()
        {
            var response = await HttpClient.PostAsync($"/events/{_eventToAdd.Id}/groupcodes/sdfsdfsd", null);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Error_Member_Did_Not_Register_In_The_Event()
        {
            var response = await HttpClient.PostAsync($"/events/{_eventToAdd.Id}/groupcodes/{_groupCode.Code}", null);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
  
        [Fact]
        public async Task Add_CurrentUser_To_Group_Code()
        {
            _eventToAdd.AddAttendance(_member, AttendanceRegisterType.CurrentEvent);
            await Context.SaveChangesAsync();
            var response = await HttpClient.PostAsync($"/events/{_eventToAdd.Id}/groupcodes/{_groupCode.Code}", null);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result =await  response.Content.ReadAsAsync<AddCurrentUserToGroupCodeCommand.Response>();
            result.Id.Should().NotBe(null);
            result.Detail.Should().NotBe(null);
        }

        private async Task FillData()
        {
            _eventToAdd = new Event();
            _member = new Member
            {
                Email = "test@test.com",
                Role = UserRole.Member
            };

            Context.Events.Add(_eventToAdd);
            Context.Members.Add(_member);

            _groupCode = _eventToAdd.CreateGroupCode("Test");
            await Context.SaveChangesAsync();

        }
    }
}
