using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Data;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class AddAttendeeShould : IntegrationTestsBase
    {
        public AddAttendeeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Add_Member_As_Attende_To_Event()
        {
            var eventToAdd = new Event();
            var member = new Member();

            Context.Events.Add(eventToAdd);
            Context.Members.Add(member);
            Context.SaveChanges();
            var response = await HttpClient.PostAsync($"/events/{eventToAdd.Id}/members/{member.Id}/attende", null);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();
            eventToAdd = Context.Events.Include(x => x.Attendees)
                                       .FirstOrDefault(x => x.Id == eventToAdd.Id);
            eventToAdd.Attendees.Count.Should().Be(1);
            eventToAdd.Attendees.First().MemberId.Should().Be(member.Id);
        }
       
    }
}
