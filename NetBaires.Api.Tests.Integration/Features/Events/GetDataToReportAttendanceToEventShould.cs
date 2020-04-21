using FluentAssertions;
using NetBaires.Data;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NetBaires.Api.ViewModels;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class GetDataToReportAttendanceToEventShould : IntegrationTestsBase
    {
        public GetDataToReportAttendanceToEventShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Return_Information_To_Inform_Attendance()
        {
            var newEvent = new Event
            {
                Title = "Event Test",
                Description = "Event Description Test"
            };
            var newMember = new Member
            {
                Email = "test@test.com",
                Role = UserRole.Member
            };
            Context.Events.Add(newEvent);
            Context.Members.Add(newMember);
            Context.SaveChanges();
            await AuthenticateAsync(newMember.Email);
            var response = await HttpClient.GetAsync($"/events/{newEvent.Id}/Attendance");
            var eventToCheck = await response.Content.ReadAsAsync<EventToReportAttendanceViewModel>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            eventToCheck.EventDetail.Title.Should().Be(newEvent.Title);
            eventToCheck.EventDetail.Description.Should().Be(newEvent.Description);
            var tokenDecode = AttendanceService.ValidateTokenToReportMyAttendance(eventToCheck.Token);
            tokenDecode.UserId.Should().Be(newMember.Id);
            tokenDecode.EventId.Should().Be(newEvent.Id);
            tokenDecode.Exp.Should().BeLessThan(DateTime.Now.ToUniversalTime().AddMinutes(6).TimeOfDay);
            tokenDecode.Exp.Should().BeMoreThan(DateTime.Now.ToUniversalTime().AddMinutes(4).TimeOfDay);

        }
    }
}