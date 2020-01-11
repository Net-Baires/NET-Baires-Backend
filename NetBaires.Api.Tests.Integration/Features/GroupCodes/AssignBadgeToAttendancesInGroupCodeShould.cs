using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Data;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.GroupCodes
{
    public class AssignBadgeToAttendancesInGroupCodeShould : IntegrationTestsBase
    {
        private GroupCode _newGroupCode;
        private Badge _newBadge;
        private Member _newMember;

        public AssignBadgeToAttendancesInGroupCodeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Add_Members_To_Badge()
        {
            FillData();
            var response = await HttpClient.PostAsync($"/groupcodes/{_newGroupCode.Id}/badges/{_newBadge.Id}", null);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();
            _newMember = Context.Members.Include(s=> s.Badges).ThenInclude(s=> s.Badge).Where(x => x.Id == _newMember.Id).FirstOrDefault();
            _newMember.Badges.Count.Should().Be(1);
            _newMember.Badges.First().Badge.Id.Should().Be(_newBadge.Id);

        }



        private void FillData()
        {
            _newGroupCode = new GroupCode("Detalle");
            _newBadge = new Badge();
            _newMember = new Member();
            Context.GroupCodes.Add(_newGroupCode);
            _newGroupCode.AddMember(_newMember, _newGroupCode.Code);
            Context.Members.Add(_newMember);
            Context.Badges.Add(_newBadge);
            Context.SaveChanges();
        }
    }
}