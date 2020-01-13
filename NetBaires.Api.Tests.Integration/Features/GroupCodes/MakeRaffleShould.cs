using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Events.AssignBadgeToAttendances;
using NetBaires.Api.Features.Events.UpdateEvent;
using NetBaires.Api.Features.GroupsCodes.UpdateGroupCode;
using NetBaires.Api.ViewModels;
using NetBaires.Api.ViewModels.GroupCode;
using NetBaires.Data;
using NetBaires.Host;
using Newtonsoft.Json;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.GroupCodes
{
    public class MakeRaffleShould : IntegrationTestsBase
    {
        private GroupCode _newGroupCode;
        private Member _newUser;

        public MakeRaffleShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Return_Two_Differents_Winners()
        {
            FillData();
            var command = new MakeRaffleCommand { CountOfWinners = 2, GroupCodeId = _newGroupCode.Id, RepeatWinners = false };
            var response = await HttpClient.PostAsync($"/groupcodes/{_newGroupCode.Id}/raffle",
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadAsAsync<List<MemberDetailViewModel>>();
            RefreshContext();

            var groupCode = Context.GroupCodes.Include(x => x.Members).Where(x => x.Id == _newGroupCode.Id).FirstOrDefault();
            var winners = groupCode.Members.Where(s => result.Select(g => g.Id).Contains(s.MemberId)).ToList();

            foreach (var winner in winners)
                winner.Winner.Should().BeTrue();
            winners.Count().Should().Be(result.Count());
            result.Count.Should().Be(2);
            result.Select(x => x.Id).Distinct().Count().Should().Be(2);
        }

        [Fact]
        public async Task Return_Differents_Winners_In_Multiples_Requests()
        {
            FillData();
            var command = new MakeRaffleCommand { CountOfWinners = 3, GroupCodeId = _newGroupCode.Id, RepeatWinners = false };
            var response1 = await HttpClient.PostAsync($"/groupcodes/{_newGroupCode.Id}/raffle",
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));

            var response2 = await HttpClient.PostAsync($"/groupcodes/{_newGroupCode.Id}/raffle",
                 new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));

            var result = await response1.Content.ReadAsAsync<List<MemberDetailViewModel>>();
            var result1 = await response2.Content.ReadAsAsync<List<MemberDetailViewModel>>();
            RefreshContext();

            result1.Where(x => result.Select(s => s.Id).Contains(x.Id)).Any().Should().BeFalse();
            result.Where(x => result1.Select(s => s.Id).Contains(x.Id)).Any().Should().BeFalse();
        }

        private void FillData()
        {
            _newGroupCode = new GroupCode("Detalle");
            _newUser = new Member { FirstName = "Juan", LastName = "LastName" };
            var member = new Member();
            var member1 = new Member();
            var member2 = new Member();
            var member3 = new Member();
            var member4 = new Member();
            var member5 = new Member();
            var member6 = new Member();
            Context.Members.Add(member);
            Context.Members.Add(member1);
            Context.Members.Add(member2);
            Context.Members.Add(member3);
            Context.Members.Add(member4);
            Context.Members.Add(member5);
            Context.Members.Add(member6);
            Context.Members.Add(_newUser);
            Context.SaveChanges();

            _newGroupCode.AddMember(_newUser, _newGroupCode.Code);
            _newGroupCode.AddMember(member, _newGroupCode.Code);
            _newGroupCode.AddMember(member1, _newGroupCode.Code);
            _newGroupCode.AddMember(member2, _newGroupCode.Code);
            _newGroupCode.AddMember(member3, _newGroupCode.Code);
            _newGroupCode.AddMember(member4, _newGroupCode.Code);
            _newGroupCode.AddMember(member5, _newGroupCode.Code);
            _newGroupCode.AddMember(member6, _newGroupCode.Code);

            Context.GroupCodes.Add(_newGroupCode);
            Context.SaveChanges();
        }
    }
}