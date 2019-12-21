using FluentAssertions;
using NetBaires.Api.Features.Badges.GetBadge;
using NetBaires.Api.Features.Members.ViewModels;
using NetBaires.Data;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Members
{
    public class GetCommunitySummaryShould : IntegrationTestsBase
    {
        public GetCommunitySummaryShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_Event_Summary()
        {
            var speaker = new Member();
            await Context.Sponsors.AddAsync(new Sponsor());
            await Context.Sponsors.AddAsync(new Sponsor());
            await Context.Members.AddAsync(new Member { Organized = true });
            await Context.Members.AddAsync(new Member { Organized = true });
            await Context.Members.AddAsync(new Member { Organized = true });
            await Context.Events.AddAsync(new Event
            {
                Attendees = new List<Attendance> {
                new Attendance {
                        Member = new Member(),
                        Speaker = true
                    },

                    new Attendance
                    {
                        Member = speaker,
                        Speaker = true
                    }
              }
            });
            await Context.Events.AddAsync(new Event
            {
                Attendees = new List<Attendance> {
                new Attendance {
                        Member = speaker,
                        Speaker = true
                    }
              }
            });
            await Context.SaveChangesAsync();

            var response = await HttpClient.GetAsync($"/community/summary");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var memberResponse = await response.Content.ReadAsAsync<GetCommunitySummaryQuery.Response>();
            memberResponse.Should().NotBeNull();
            memberResponse.LastEvents.Count.Should().Be(2);
            memberResponse.Speakers.Count.Should().Be(2);
            memberResponse.Organizers.Count.Should().Be(3);
            memberResponse.Sponsors.Count.Should().Be(2);
            memberResponse.TotalEvents.Should().Be(2);
            memberResponse.TotalSpeakers.Should().Be(2);
            memberResponse.TotalUsersMeetup.Should().Be(8);
        }

    }
}
