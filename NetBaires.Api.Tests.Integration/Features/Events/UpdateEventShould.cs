using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NetBaires.Api.Features.Events.UpdateEvent;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Events
{
    public class UpdateEventShould : IntegrationTestsBase
    {
        private Event _newEvent;
        private Sponsor _firstSponsor;
        private Sponsor _secondSponsor;
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
                Done = true,
                GeneralAttended = true,
                Online = true,
                OnlineLink = "https://randomlink.com"
            };

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            RefreshContext();
            var eventToCheck = Context.Events.First();
            eventToCheck.Description.Should().Be(update.Description);
            eventToCheck.Title.Should().Be(update.Title);
            eventToCheck.Online.Should().BeTrue();
            eventToCheck.ImageUrl.Should().Be(update.ImageUrl);
            eventToCheck.OnlineLink.Should().Be(update.OnlineLink);
            eventToCheck.Url.Should().Be(update.Url);
            eventToCheck.GeneralAttended.Should().Be(update.GeneralAttended.Value);
            eventToCheck.Done.Should().BeTrue();
            eventToCheck.Live.Should().BeFalse();

        }

        [Fact]
        public async Task Add_Sponsors()
        {
            FillData();

            var update = new UpdateEventCommand
            {
                Sponsors = new List<SponsorEventViewModel>
                {
                    new SponsorEventViewModel{ SponsorId=2, Detail="Detail test"}
                }
            };

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            RefreshContext();
            var eventToCheck = Context.Events.Include(x => x.Sponsors).First();
            eventToCheck.Sponsors.Count.Should().Be(1);

        }
        [Fact]
        public async Task Remove_All_Sponsors()
        {
            FillData();
            _newEvent.AddSponsor(_firstSponsor, "Razón");
            _newEvent.AddSponsor(_secondSponsor, "Razón");

            Context.SaveChanges();
            var update = new UpdateEventCommand
            {
                Sponsors = new List<SponsorEventViewModel>()
            };

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            RefreshContext();
            var eventToCheck = Context.Events.Include(x => x.Sponsors).First();
            eventToCheck.Sponsors.Count.Should().Be(0);

        }
        [Fact]
        public async Task Remove_A_Sponsors()
        {
            FillData();
            _newEvent.AddSponsor(_firstSponsor, "Razón");
            _newEvent.AddSponsor(_secondSponsor, "Razón");
            Context.SaveChanges();
            var update = new UpdateEventCommand
            {
                Sponsors = new List<SponsorEventViewModel> { new SponsorEventViewModel { SponsorId = _secondSponsor.Id, Detail = "Yeahh" } }
            };

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            RefreshContext();
            var eventToCheck = Context.Events.Include(x => x.Sponsors).First();
            eventToCheck.Sponsors.Count.Should().Be(1);
            eventToCheck.Sponsors.First().Detail.Should().Be(update.Sponsors.First().Detail);

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
        public async Task Enable_event_General_Attendance()
        {
            FillData();

            var update = new UpdateEventCommand
            {
                GeneralAttended = true
            };

            var response = await HttpClient.PutAsync($"/events/{_newEvent.Id}",
                new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            RefreshContext();
            var eventToCheck = Context.Events.First();
            eventToCheck.GeneralAttended.Should().BeTrue();
            eventToCheck.GeneralAttendedCode.Should().NotBeNull();
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
            _firstSponsor = new Sponsor { Name = "Sponsor 1" };
            _secondSponsor = new Sponsor { Name = "Sponsor 2" };
            Context.Sponsors.Add(_firstSponsor);
            Context.Sponsors.Add(_secondSponsor);
            Context.Events.Add(_newEvent);
            Context.SaveChanges();
        }
    }
}