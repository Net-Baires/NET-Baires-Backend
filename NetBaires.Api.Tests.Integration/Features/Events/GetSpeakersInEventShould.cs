using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NetBaires.Api.Features.Events.GetSpeakersInEvent;
using NetBaires.Data;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;
namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class GetSpeakersInEventShould : IntegrationTestsBase
    {
        private Event _event;

        public GetSpeakersInEventShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }


        [Fact]
        public async Task Return_All_Speakers()
        {
            FillData();
            _event.AddSpeaker(new Member { FirstName = "Test Name" });
            Context.SaveChanges();
            _event.AddSpeaker(new Member { FirstName = "Test Name 2" });
            Context.SaveChanges();
            _event.AddSpeaker(new Member { FirstName = "Test Name 3" });

            _event.AddAttendance(new Member(), AttendanceRegisterType.CurrentEvent);
            _event.AddAttendance(new Member(), AttendanceRegisterType.CurrentEvent);
            Context.SaveChanges();
            RefreshContext();
            var aa = Context.Attendances.Include(x => x.Member).ToList();

            var response = await HttpClient.GetAsync($"/events/{_event.Id}/speakers");
            var events = await response.Content.ReadAsAsync<List<GetSpeakersInEventInEventResponse>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            events.Count.Should().Be(3);
            events.Any(x => x.Member.FirstName == "Test Name").Should().BeTrue();
            events.Any(x => x.Member.FirstName == "Test Name 2").Should().BeTrue();
            events.Any(x => x.Member.FirstName == "Test Name 3").Should().BeTrue();


        }

        private void FillData()
        {
            _event = new Event
            {
                Live = true
            };
            Context.Events.Add(_event);
            Context.SaveChanges();
        }
    }
}