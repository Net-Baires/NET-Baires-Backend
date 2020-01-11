using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Features.Events.GetEventLiveDetail;
using NetBaires.Data;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class GetEventLiveDetailShould : IntegrationTestsBase
    {
        public GetEventLiveDetailShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
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
            var secondAttended = new Member
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
                Platform = EventPlatform.Meetup,
                Date = DateTime.Now,
                Attendees = new List<Attendance> {
                    new Attendance{
                            Member= firstAttended,
                            AttendedTime=DateTime.Now,
                            Attended= true,
                    },
                     new Attendance{
                            Member= secondAttended ,
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
            var groupCode = newEventLive.CreateGroupCode("Detail");
            var groupCode2 = newEventLive.CreateGroupCode("Detail2");
            groupCode.AddMember(firstAttended, groupCode.Code);
            groupCode2.AddMember(secondAttended, groupCode2.Code);
            groupCode2.AddMember(firstAttended, groupCode2.Code);

            Context.Events.Add(newEventLive);
            Context.SaveChanges();
            var response = await HttpClient.GetAsync($"/events/{newEventLive.Id}/Live/Detail");
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
            eventDetail.GroupCodes.Count.Should().Be(2);
            eventDetail.GroupCodes.First().Code.Should().Be(groupCode.Code);
            eventDetail.GroupCodes.First().Detail.Should().Be(groupCode.Detail);
            eventDetail.GroupCodes.First().Id.Should().Be(groupCode.Id);
            eventDetail.GroupCodes.First().MembersCount.Should().Be(groupCode.Members.Count);

            eventDetail.GroupCodes[1].Code.Should().Be(groupCode2.Code);
            eventDetail.GroupCodes[1].Detail.Should().Be(groupCode2.Detail);
            eventDetail.GroupCodes[1].Id.Should().Be(groupCode2.Id);
            eventDetail.GroupCodes[1].MembersCount.Should().Be(groupCode2.Members.Count);



            eventDetail.MembersDetails.MembersAttended[1].FirstName.Should().Be(firstAttended.FirstName);
            eventDetail.MembersDetails.MembersAttended[1].LastName.Should().Be(firstAttended.LastName);
            eventDetail.MembersDetails.MembersAttended[1].Picture.Should().Be(firstAttended.Picture);
            eventDetail.MembersDetails.MembersAttended[1].Username.Should().Be(firstAttended.Username);

            eventDetail.MembersDetails.MembersAttended[0].FirstName.Should().Be(secondAttended.FirstName);
            eventDetail.MembersDetails.MembersAttended[0].LastName.Should().Be(secondAttended.LastName);
            eventDetail.MembersDetails.MembersAttended[0].Picture.Should().Be(secondAttended.Picture);
            eventDetail.MembersDetails.MembersAttended[0].Username.Should().Be(secondAttended.Username);

        }

    }
}