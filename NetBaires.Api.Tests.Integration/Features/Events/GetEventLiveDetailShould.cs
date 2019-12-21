using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Features.Events.ViewModels;
using NetBaires.Api.Handlers.Events;
using NetBaires.Data;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Events
{
    public class GetEventLiveDetailShould : IntegrationTestsBase
    {
        private Event _event;

        public GetEventLiveDetailShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_204_Empty_Events()
        {
            var response = await HttpClient.GetAsync("/events");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Return_Live_EventDetail()
        {
            var firstAttended = new Member
            {
                FirstName = "First Member",
                LastName = "First Member Last Name",
                Picture = "PictureTest",
                Username = "FirstUserName"
            };
            var SecondAttended = new Member
            {
                FirstName = "Second Member Attended",
                LastName = "Second Member Last Name",
                Picture = "PictureTest2",
                Username = "FirstUserName2"
            };
            var newEventLive = new Event
            {
                Live = true,
                Title = "Title",
                Description = "Description",
                Platform= EventPlatform.Meetup,
                Date= DateTime.Now,
                Attendees = new List<Attendance> {
                    new Attendance{
                            Member= firstAttended,
                            AttendedTime=DateTime.Now,
                            Attended= true,
                    },
                     new Attendance{
                            Member= SecondAttended ,
                            AttendedTime=DateTime.Now,
                            Attended= true,
                    },
                     new Attendance{
                            Member= new Member{
                                FirstName="No Attended Member",
                                LastName="No Attended Member Last Name"
                            },
                            AttendedTime=DateTime.Now
                    }
                }
            };
            Context.Events.Add(newEventLive);
            Context.SaveChanges();
            var response = await HttpClient.GetAsync($"/events/{newEventLive.Id}/LiveDetail");
            var eventDetail = await response.Content.ReadAsAsync<GetEventLiveDetailQuery.Response>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            eventDetail.Id.Should().Be(newEventLive.Id);
            eventDetail.Title.Should().Be(newEventLive.Title);
            eventDetail.Description.Should().Be(newEventLive.Description);
            eventDetail.Platform.Should().Be(newEventLive.Platform);
            eventDetail.ImageUrl.Should().Be(newEventLive.ImageUrl);
            eventDetail.MembersDetails.TotalMembersAttended.Should().Be(2);
            eventDetail.MembersDetails.TotalMembersRegistered.Should().Be(3);
            eventDetail.MembersDetails.MembersAttended.Count.Should().Be(2);
            eventDetail.MembersDetails.MembersAttended[0].FirstName.Should().Be(firstAttended.FirstName);
            eventDetail.MembersDetails.MembersAttended[0].LastName.Should().Be(firstAttended.LastName);
            eventDetail.MembersDetails.MembersAttended[0].Picture.Should().Be(firstAttended.Picture);
            eventDetail.MembersDetails.MembersAttended[0].Username.Should().Be(firstAttended.Username);

            eventDetail.MembersDetails.MembersAttended[1].FirstName.Should().Be(SecondAttended.FirstName);
            eventDetail.MembersDetails.MembersAttended[1].LastName.Should().Be(SecondAttended.LastName);
            eventDetail.MembersDetails.MembersAttended[1].Picture.Should().Be(SecondAttended.Picture);
            eventDetail.MembersDetails.MembersAttended[1].Username.Should().Be(SecondAttended.Username);

        }

    }
}