using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Events.ViewModels;
using NetBaires.Api.Handlers.Events;
using NetBaires.Api.Models;
using NetBaires.Data;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class UpdateEventShould : IntegrationTestsBase
    {
        private Event _newEvent;
        private Member _newMember;
        private Attendance _attendance;

        public UpdateEventShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Update_Event_Details()
        {
            FillData();

            var update = new UpdateEventCommand
            {
                Description = "Description 2",
                Title = "Title 2",
                ImageUrl = "ImageUrl 2",
                Url = "Url 2",
                Done = true
            };

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            RefreshContext();
            var eventToCheck = Context.Events.First();
            eventToCheck.Description.Should().Be(update.Description);
            eventToCheck.Title.Should().Be(update.Title);
            eventToCheck.ImageUrl.Should().Be(update.ImageUrl);
            eventToCheck.Url.Should().Be(update.Url);
            eventToCheck.Done.Should().BeTrue();
            eventToCheck.Live.Should().BeFalse();

        }
        [Fact]
        public async Task Set_Event_Live()
        {
            FillData();

            var update = new UpdateEventCommand
            {
                Live = true
            };

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            RefreshContext();
            var eventToCheck = Context.Events.First();
            eventToCheck.Live.Should().BeTrue();
            eventToCheck.StartLiveTime.Should().NotBeNull();
        }
        [Fact]
        public async Task Set_Event_UnLive()
        {
            FillData();

            var update = new UpdateEventCommand
            {
                Live = false
            };

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            RefreshContext();
            var eventToCheck = Context.Events.First();
            eventToCheck.Live.Should().BeFalse();
            eventToCheck.EndLiveTime.Should().NotBeNull();
        }

        private void FillData()
        {
            _newEvent = new Event
            {
                Date = DateTime.Now,
                Description = "Description",
                ImageUrl = "ImageUrl",
                Live = false,
                Platform = EventPlatform.EventBrite,
                Title = "Title",
                Url = "Meetupurl"
            };
            Context.Events.Add(_newEvent);
            Context.SaveChanges();
        }
    }
}