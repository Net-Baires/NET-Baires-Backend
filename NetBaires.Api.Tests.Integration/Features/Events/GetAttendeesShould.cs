using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.ViewModels;
using NetBaires.Data;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class GetAttendeesShould : IntegrationTestsBase
    {
        public GetAttendeesShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Return_All_Attendees()
        {
            var newEvent = new Event();
            var newMember = new Member
            {
                Email = "test@test.com",
                Biography = "Biography",
                FirstName = "FirstName",
                Github = "Github",
                Instagram = "Instagram",
                LastName = "LastName",
                Linkedin = "Linkedin"
            };
            newEvent.AddAttendance(newMember);
            newEvent.AddAttendance(new Member());
            newEvent.AddAttendance(new Member());
            Context.Events.Add(newEvent);
            Context.SaveChanges();
            var response = await HttpClient.GetAsync($"/events/{newEvent.Id}/attendees");
            var attendants = await response.Content.ReadAsAsync<List<AttendantViewModel>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            attendants.Count.Should().Be(3);
            attendants.First().MemberDetail.Email.Should().Be(newMember.Email);
            attendants.First().MemberDetail.Biography.Should().Be(newMember.Biography);
            attendants.First().MemberDetail.FirstName.Should().Be(newMember.FirstName);
            attendants.First().MemberDetail.Github.Should().Be(newMember.Github);
            attendants.First().MemberDetail.Instagram.Should().Be(newMember.Instagram);
            attendants.First().MemberDetail.LastName.Should().Be(newMember.LastName);
            attendants.First().MemberDetail.Linkedin.Should().Be(newMember.Linkedin);
            attendants.First().MemberDetail.FirstName.Should().Be(newMember.FirstName);
        }
    }
}