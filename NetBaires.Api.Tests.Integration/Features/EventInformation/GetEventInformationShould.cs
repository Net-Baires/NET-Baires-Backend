using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Features.EventInformation.GetEventInformation;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.EventInformation
{
    public class GetEventInformationShould : IntegrationTestsBase
    {
        private Event _event;
        private Data.Entities.EventInformation _eventInformation;

        public GetEventInformationShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }


        [Fact]
        public async Task Get_All_EventInformation()
        {
            FillData();

            var response = await HttpClient.GetAsync($"/events/{_event.Id}/information");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var materials = await response.Content.ReadAsAsync<List<EventInformationViewModel>>();


            materials.Count.Should().Be(3);
            materials.Any(x => x.Id == _eventInformation.Id
                               &&
                               x.Title == _eventInformation.Title
                               &&
                               x.Description == _eventInformation.Description
                                   && x.Visible == _eventInformation.Visible).Should().BeTrue();
        }

        private void FillData()
        {
            _event = new Event();
            _event.AddInformation("Title 1", "http://link.com",true);
            _event.AddInformation("Title 2", "http://link.com",true);
            _event.AddInformation("Title 3", "http://link.com",true);
            Context.Events.Add(_event);
            Context.SaveChanges();
            _eventInformation = _event.Information.First();
        }

    }
}