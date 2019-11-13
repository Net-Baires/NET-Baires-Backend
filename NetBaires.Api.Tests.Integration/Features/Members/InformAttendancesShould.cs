using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Members
{
    public class InformAttendancesShould : IntegrationTestsBase
    {
        public InformAttendancesShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Add_New_Member_Attended()
        {
            var newEvent = new Event();
            var member = new Member();
            Context.Members.Add(member);
            Context.Events.Add(newEvent);

            await Context.SaveChangesAsync();

            var response = await HttpClient.PutAsync($"/members/{member.Id}/Events/{newEvent.Id}/Attendances/true", null);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            newEvent = Context.Events.Include(x => x.Attendees).First();
            newEvent.Attendees.Count.Should().Be(1);
            newEvent.Attendees.First().MemberId.Should().Be(member.Id);
            newEvent.Attendees.First().Attended.Should().BeTrue();
            newEvent.Attendees.First().DidNotAttend.Should().BeFalse();
        }
        [Fact]
        public async Task Add_New_Member_No_Attended()
        {
            var newEvent = new Event();
            var member = new Member();
            Context.Members.Add(member);
            Context.Events.Add(newEvent);

            await Context.SaveChangesAsync();

            var response = await HttpClient.PutAsync($"/members/{member.Id}/Events/{newEvent.Id}/Attendances/false", null);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            newEvent = Context.Events.Include(x => x.Attendees).First();
            newEvent.Attendees.Count.Should().Be(1);
            newEvent.Attendees.First().MemberId.Should().Be(member.Id);
            newEvent.Attendees.First().Attended.Should().BeFalse();
            newEvent.Attendees.First().DidNotAttend.Should().BeTrue();
        }
    }
}