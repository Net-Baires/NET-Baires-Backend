using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Data;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.GroupCodes
{
    public class AddMemberToGroupCodeShould : IntegrationTestsBase
    {
        private GroupCode _groupCode;
        private Event _eventToAdd;
        private Member _member;

        public AddMemberToGroupCodeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            FillData().GetAwaiter().GetResult();
            AuthenticateAsync(_member.Email).GetAwaiter().GetResult();

        }

        [Fact]
        public async Task Not_Found_Invalid_Code()
        {
            var response = await HttpClient.PostAsync($"/groupcodes/{_groupCode.Id}/sdfsdfsd", null);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Error_Member_Did_Not_Register_In_The_Event()
        {
            var response = await HttpClient.PostAsync($"/groupcodes/{_groupCode.Id}/{_groupCode.Code}", null);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Not_Found_Invalid_Id()
        {
            var response = await HttpClient.PostAsync($"/groupcodes/7/{_groupCode.Code}", null);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task Add_Member_To_Group_Code()
        {
            _eventToAdd.AddAttendance(_member, AttendanceRegisterType.CurrentEvent);
            await Context.SaveChangesAsync();
            var response = await HttpClient.PostAsync($"/groupcodes/{_groupCode.Id}/{_groupCode.Code}", null);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();
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
