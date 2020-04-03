using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Speakers.GetSpeaker;
using NetBaires.Data;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Speakers
{
    public class GetSpeakerShould : IntegrationTestsBase
    {
        public GetSpeakerShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_One_Speaker_Detail_With_3_Events()
        {
            var member = new Member();
            var event1 = new Event();
            var event2 = new Event();
            var event3 = new Event();
            var event4 = new Event();
            event1.AddSpeaker(member);
            event2.AddSpeaker(member);
            event3.AddSpeaker(member);
            Context.Events.AddRange(new List<Event> { event1, event2, event3, event4 });
            Context.SaveChanges();
            var memebrs = Context.Members.Include(x => x.Events).ToList();
            var response = await HttpClient.GetAsync($"/speakers/{member.Id}");
            var speakers = await response.Content.ReadAsAsync<GetSpeakerResponse>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            speakers.CountEventsAsSpeaker.Should().Be(3);
            speakers.Events.Count.Should().Be(3);
            speakers.Events.Any(x => x.Id == event1.Id);
        }

    }
}