using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Events.AddMemberToGroupCode;
using NetBaires.Data;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class AddMemberToGroupCodeShould : IntegrationTestsBase
    {
        private GroupCode _groupCode;
        private Event _eventToAdd;
        private Member _member;

        public AddMemberToGroupCodeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            FillData().GetAwaiter().GetResult();
            AuthenticateAdminAsync().GetAwaiter().GetResult();

        }
  
        [Fact]
        public async Task Add_Member_To_Group_Code()
        {
            _eventToAdd.AddAttendance(_member, AttendanceRegisterType.CurrentEvent);
            await Context.SaveChangesAsync();
            var response = await HttpClient.PostAsync($"/events/{_eventToAdd.Id}/groupcodes/{_groupCode.Id}/members/{_member.Id}", null);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result =await  response.Content.ReadAsAsync<AddMemberToGroupCodeCommand.Response>();
            RefreshContext();
            var groupCode = await Context.GroupCodes.Include(x=> x.Members).FirstOrDefaultAsync(x => x.Id == _groupCode.Id);
            var eventComplete = await Context.Events.Include(x => x.Attendees)
                .FirstOrDefaultAsync(s => s.Id == _eventToAdd.Id);
            eventComplete.Attendees.FirstOrDefault(x => x.MemberId == _member.Id).Attended.Should().BeTrue();
            groupCode.Members.Any(s => s.MemberId == _member.Id).Should().BeTrue();
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
