using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Data;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Services
{

    public class EventBriteSyncServicesShould : IntegrationTestsBase
    {
        private Event _event;
        public EventBriteSyncServicesShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }

        private void FillData(string email)
        {
            _event = new Event
            {
                EventId = "1234",
                Platform = EventPlatform.EventBrite,
                Attendees = new List<Attendance>{
                    new Attendance{
                        Member = new Member{
                            Email= email
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
            FillData("primero@primero.com");

            await SyncServices.SyncEvent(_event.Id);
            var attendees = Context.Attendances.Include(x => x.Member).Where(x => x.EventId == _event.Id).ToList();

            var noAttended = attendees.FirstOrDefault(x => x.Member.Email == "NoAsisto@NoAsistio.com");
            var attended = attendees.FirstOrDefault(x => x.Member.Email == "Asisto@Asistio.com");
            attendees.Count.Should().Be(3);
            noAttended.DidNotAttend.Should().BeTrue();

            attended.DidNotAttend.Should().BeFalse();
            attended.Attended.Should().BeTrue();
        }

        [Fact]
        public async Task Update_One_Attendant_In_EventBrite_Attended()
        {
            FillData("Asisto@Asistio.com");

            await SyncServices.SyncEvent(_event.Id);
            var attendees = Context.Attendances.Include(x => x.Member).Where(x => x.EventId == _event.Id).ToList();

            var attended = attendees.FirstOrDefault(x => x.Member.Email == "Asisto@Asistio.com");
            attended.DidNotAttend.Should().BeFalse();
            attended.Attended.Should().BeTrue();
        }
    }
}