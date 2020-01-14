using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Data;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Services
{

    public class MeetupSyncServicesShould : IntegrationTestsBase
    {
        private Event _event;
        public MeetupSyncServicesShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }

        private void FillData(long meetupId)
        {
            _event = new Event
            {
                EventId = "1234",
                Platform = EventPlatform.Meetup,
                Attendees = new List<Attendance>{
                    new Attendance{
                        Member = new Member{
                            MeetupId = meetupId
                        },
                        Attended = false
                    }
                }

            };
            Context.Events.Add(_event);
            Context.SaveChanges();
        }
        [Fact]
        public async Task Add_Two_New_Attendees_One_Attended_One_No_Attended()
        {
            FillData(0000);

            await SyncServices.SyncEvent(_event.Id);
            RefreshContext();
            var attendees = Context.Attendances.Include(x => x.Member).Where(x => x.EventId == _event.Id).ToList();

            var noAttended = attendees.FirstOrDefault(x => x.Member.MeetupId == 1234567);
            var attended = attendees.FirstOrDefault(x => x.Member.MeetupId== 123456);
            attendees.Count.Should().Be(4);
            noAttended.DidNotAttend.Should().BeTrue();

            attended.DidNotAttend.Should().BeFalse();
            attended.Attended.Should().BeTrue();
        }

        [Fact]
        public async Task Update_One_Attendant_In_EventBrite_Attended()
        {
            FillData(123456);

            await SyncServices.SyncEvent(_event.Id);
            var attendees = Context.Attendances.Include(x => x.Member).Where(x => x.EventId == _event.Id).ToList();

            var attended = attendees.FirstOrDefault(x => x.Member.MeetupId == 123456);
            attended.DidNotAttend.Should().BeFalse();
            attended.Attended.Should().BeTrue();
        }
    }
}