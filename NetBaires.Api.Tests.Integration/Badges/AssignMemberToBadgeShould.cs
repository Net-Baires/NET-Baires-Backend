using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using FluentAssertions;
using System.Net;
using System.Linq;
using System;
using NetBaires.Api.Handlers.Badges;
using System.IO;
using NetBaires.Data;

namespace NetBaires.Api.Tests.Integration
{
    public class AssignMemberToBadgeShould : IntegrationTestsBase
    {
        private Member _member;
        private Badge _badge;

        public AssignMemberToBadgeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
            FillData();
        }

        private void FillData()
        {
            _member = new Member
            {
                Email = "Email@email.com"
            };
            _badge = new Badge
            {
                Name = "test"
            };
            Context.Members.Add(_member);
            Context.Badges.Add(_badge);
            Context.SaveChanges();
            Context.SaveChanges();
        }

        [Fact]
        public async Task Assign_Badge_To_Member()
        {
            var response = await HttpClient.PostAsync($"/badges/{_badge.Id}/members/{_member.Id}", null);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var badgeInMember = Context.BadgeMembers.FirstOrDefault();
            badgeInMember.BadgeId.Should().Be(_badge.Id);
            badgeInMember.MemberId.Should().Be(_member.Id);
        }
        [Fact]
        public async Task Not_Assign_Badge_To_Member_Member_Already_Exist()
        {
            await HttpClient.PostAsync($"/badges/{_badge.Id}/members/{_member.Id}", null);
            var response = await HttpClient.PostAsync($"/badges/{_badge.Id}/members/{_member.Id}", null);
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }
    }
}