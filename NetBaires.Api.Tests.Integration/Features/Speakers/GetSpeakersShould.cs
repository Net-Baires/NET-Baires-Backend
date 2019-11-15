using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Handlers.Speakers;
using NetBaires.Data;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Speakers
{
    public class GetSpeakersShould : IntegrationTestsBase
    {
        public GetSpeakersShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_One_Speaker_With_3_Events()
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
            var response = await HttpClient.GetAsync("/speakers");
            var speakers = await response.Content.ReadAsAsync<List<GetSpeakersResponse>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            speakers.Count.Should().Be(1);
            speakers.First().CounEventsAsSpeaker.Should().Be(3);
        }

    }
}