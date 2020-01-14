using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Events.AddMemberToGroupCode;
using NetBaires.Api.Features.Events.DeleteMemberToGroupCode;
using NetBaires.Data;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class DeleteMemberToGroupCodeShould : IntegrationTestsBase
    {
        private GroupCode _groupCode;
        private Event _eventToAdd;
        private Member _member;

        public DeleteMemberToGroupCodeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            FillData().GetAwaiter().GetResult();
            AuthenticateAdminAsync().GetAwaiter().GetResult();

        }
        [Fact]
        public async Task Delete_Member_From_Group_Code()
        {
            _eventToAdd.AddAttendance(_member, AttendanceRegisterType.CurrentEvent);
            await Context.SaveChangesAsync();
            var response = await HttpClient.DeleteAsync($"/events/{_eventToAdd.Id}/groupcodes/{_groupCode.Id}/members/{_member.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();
            var groupCode = await Context.GroupCodes.Include(x => x.Members).FirstOrDefaultAsync(x => x.Id == _groupCode.Id);
            groupCode.Members.Any(s => s.MemberId == _member.Id).Should().BeFalse();
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
            await Context.SaveChangesAsync();

            _groupCode = _eventToAdd.CreateGroupCode("Test");
            _groupCode.AddMember(_member);
            await Context.SaveChangesAsync();

        }
    }
}
