using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Events.UpdateEvent;
using NetBaires.Api.Features.GroupsCodes.UpdateGroupCode;
using NetBaires.Api.ViewModels.GroupCode;
using NetBaires.Data;
using NetBaires.Host;
using Newtonsoft.Json;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.GroupCodes
{
    public class GetGroupCodesShould : IntegrationTestsBase
    {
        private GroupCode _newGroupCode;
        private Member _newUser;

        public GetGroupCodesShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Get_GroupCode_Full_Detail()
        {
            FillData();
            var response = await HttpClient.GetAsync($"/groupcodes/{_newGroupCode.Id}");
            var groupCode = await response.Content.ReadAsAsync<GroupCodeFullDetailResponseViewModel>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            RefreshContext();

            groupCode.Members.Count.Should().Be(2);
            groupCode.Detail.Should().Be(_newGroupCode.Detail);
            groupCode.Members.Any(x => x.FirstName == _newUser.FirstName).Should().BeTrue();
            groupCode.Members.Any(x => x.LastName == _newUser.LastName).Should().BeTrue();
            groupCode.Members.Any(x => x.Picture == _newUser.Picture).Should().BeTrue();
        }

        private void FillData()
        {
            _newGroupCode = new GroupCode("Detalle");
            _newUser = new Member { FirstName = "Juan", LastName = "LastName" };
            var member = new Member();
            Context.Members.Add(member);
            Context.Members.Add(_newUser);
            Context.SaveChanges();

            _newGroupCode.AddMember(_newUser, _newGroupCode.Code);

            _newGroupCode.AddMember(member, _newGroupCode.Code);
            Context.GroupCodes.Add(_newGroupCode);
            Context.SaveChanges();
        }
    }
}