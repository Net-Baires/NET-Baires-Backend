﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Auth;
using NetBaires.Api.Services;
using NetBaires.Data;
using NetBaires.Data.DomainEvents;
using NetBaires.Data.Entities;
using NetBaires.Events.DomainEvents;
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
            _newMember = Context.Members.Include(s => s.Badges).ThenInclude(s => s.Badge).Where(x => x.Id == _newMember.Id).FirstOrDefault();
            _newMember.Badges.Count.Should().Be(1);
            _newMember.Badges.First().Badge.Id.Should().Be(_newBadge.Id);

        }

        [Fact]
        public async Task Add_Members_To_Badge_Add_Message_To_The_Queue()
        {
            QueueServices.Clear<AssignedBadgeToMember>();
            FillData();

            var a = await HttpClient.PostAsync($"/groupcodes/{_newGroupCode.Id}/badges/{_newBadge.Id}", null);

            var bb = await a.Content.ReadAsStringAsync();
            var message = QueueServices.GetMessage<AssignedBadgeToMember>();
            message.MemberId.Should().NotBe(0);
            message.BadgeId.Should().Be(_newBadge.Id);
            Action act = () => QueueServices.GetMessage<AssignedBadgeToMember>();
            act.Should().Throw<NullReferenceException>();

            QueueServices.Clear<AssignedBadgeToMember>();

        }
        [Fact]
        public async Task Not_Assign_Two_Time_Same_Badge_To_The_Group()
        {
            FillData();
            await HttpClient.PostAsync($"/groupcodes/{_newGroupCode.Id}/badges/{_newBadge.Id}", null);
            var response = await HttpClient.PostAsync($"/groupcodes/{_newGroupCode.Id}/badges/{_newBadge.Id}", null);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            RefreshContext();
            _newMember = Context.Members.Include(s => s.Badges).ThenInclude(s => s.Badge).Where(x => x.Id == _newMember.Id).FirstOrDefault();
            _newMember.Badges.Count.Should().Be(1);
            _newMember.Badges.First().Badge.Id.Should().Be(_newBadge.Id);

        }

        [Fact]
        public async Task Not_Assign_Two_Time_Same_Badge_To_The_Same_Member()
        {
            FillData();
            _newMember.AssignBadge(_newBadge);
            Context.SaveChanges();
            await HttpClient.PostAsync($"/groupcodes/{_newGroupCode.Id}/badges/{_newBadge.Id}", null);
            var response = await HttpClient.PostAsync($"/groupcodes/{_newGroupCode.Id}/badges/{_newBadge.Id}", null);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            RefreshContext();
            _newMember = Context.Members.Include(s => s.Badges).ThenInclude(s => s.Badge).Where(x => x.Id == _newMember.Id).FirstOrDefault();
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