﻿using FluentAssertions;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Members.SearchMember;
using NetBaires.Api.Services.Meetup.Models;
using NetBaires.Host;
using Xunit;
using Member = NetBaires.Data.Entities.Member;

namespace NetBaires.Api.Tests.Integration.Features.Members
{
    public class FollowMemberShould : IntegrationTestsBase
    {
        public FollowMemberShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Follow_New_Member()
        {
            var memberToFollow = new Member();
            Context.Members.Add(memberToFollow);
            Context.SaveChanges();

            var response = await HttpClient.PostAsync($"/members/{memberToFollow.Id}/follow", null);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();

            var member = Context.Members.Include(x => x.FollowingMembers)
                                        .ThenInclude(x=> x.Following)
                                        .First(x => x.Email == "admin@admin.com");

            member.FollowingMembers.Count.Should().Be(1);
            member.FollowingMembers.First().Following.Should().Be(memberToFollow);
        }

       
    }
}
