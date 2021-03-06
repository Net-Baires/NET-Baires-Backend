﻿using FluentAssertions;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetBaires.Host;
using Xunit;
using Member = NetBaires.Data.Entities.Member;

namespace NetBaires.Api.Tests.Integration.Features.Members
{
    public class UnFollowMemberShould : IntegrationTestsBase
    {
        public UnFollowMemberShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task UnFollow_New_Member()
        {
            var memberToFollow = new Member();
            Context.Members.Add(memberToFollow);
            Context.SaveChanges();
            var member = Context.Members.Include(x => x.FollowingMembers)
                .ThenInclude(x => x.Following)
                .First(x => x.Email == "admin@admin.com");
            member.Follow(memberToFollow);
            Context.SaveChanges();

            var response = await HttpClient.DeleteAsync($"/members/{memberToFollow.Id}/unFollow");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();
            member = Context.Members.Include(x => x.FollowingMembers)
               .ThenInclude(x => x.Following)
               .First(x => x.Email == "admin@admin.com");

            member.FollowingMembers.Count.Should().Be(0);
        }

    }
}
