using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Features.Events.GetLinkEventLive;
using NetBaires.Data;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class GetLinkEventLiveShould : IntegrationTestsBase
    {
        private Event _onlineEvent;

        public GetLinkEventLiveShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Return_204_Empty_Events()
        {
            var response = await HttpClient.GetAsync("/events/live/link");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_Link_Only_Event_Live()
        {
            FillData();
            var currentMember = Context.Members.First();
            _onlineEvent.AddAttendance(currentMember, AttendanceRegisterType.CurrentEvent);
            Context.SaveChanges();
            var response = await HttpClient.GetAsync("/events/live/link");
            var eventResult = await response.Content.ReadAsAsync<GetLinkEventLiveQuery.Response>();
            eventResult.OnlineLink.Should().Be(_onlineEvent.OnlineLink);
        }

        private void FillData()
        {

            _onlineEvent = new Event
            {
                Live = true,
                Online = true,
                OnlineLink = "http://testlink.com"
            };
            Context.Events.Add(_onlineEvent);
            Context.Events.Add(new Event
            {
                Live = true
            });
            Context.SaveChanges();
        }
    }
}