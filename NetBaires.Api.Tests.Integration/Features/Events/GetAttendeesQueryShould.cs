using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.ViewModels;
using NetBaires.Data;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class GetAttendeesQueryShould : IntegrationTestsBase
    {
        private Event _event;

        public GetAttendeesQueryShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }



        [Fact]
        public async Task Return_All_Attendees()
        {
            FillData();
            var currentMember = Context.Members.First();
            _event.AddAttendance(currentMember, AttendanceRegisterType.CurrentEvent);
            Context.SaveChanges();
            var response = await HttpClient.GetAsync($"/events/{_event.Id}/attendees");
            var eventResult = await response.Content.ReadAsAsync<List<AttendantViewModel>>();
            eventResult.Count.Should().Be(3);
        }
        [Fact]
        public async Task Return_One_Attendees()
        {
            FillData();
            var currentMember = Context.Members.First();
            _event.AddAttendance(currentMember, AttendanceRegisterType.CurrentEvent);
            Context.SaveChanges();
            var response = await HttpClient.GetAsync($"/events/{_event.Id}/attendees?memberId={_event.Attendees[1].MemberId}");
            var eventResult = await response.Content.ReadAsAsync<AttendantViewModel>();
            eventResult.MemberDetail.FirstName.Should().Be(_event.Attendees[1].Member.FirstName);
        }


        private void FillData()
        {
            Context.Events.Add(new Event());
            Context.Events.Add(new Event());

            _event = new Event
            {
                Live = true,
                Attendees = new List<Attendance> {
                new Attendance {
                     Member = new Member { FirstName="Test 1" }

                },
                new Attendance
                {
                    Member = new Member { FirstName = "Test 2" }
                }
               }
            };
            Context.Events.Add(_event);
            Context.SaveChanges();
        }
    }
}