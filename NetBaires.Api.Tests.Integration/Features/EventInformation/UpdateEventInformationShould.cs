using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.EventInformation.AddEventInformation;
using NetBaires.Api.Features.EventInformation.GetEventInformation;
using NetBaires.Api.Features.EventInformation.UpdateEventInformation;
using NetBaires.Api.Features.Materials.AddMaterial;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Newtonsoft.Json;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.EventInformation
{
    public class UpdateEventInformationShould : IntegrationTestsBase
    {
        private Event _event;
        private Data.Entities.EventInformation _eventInformation;

        public UpdateEventInformationShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }


        [Fact]
        public async Task Update_Event_Information()
        {
            FillData();

            var command = new UpdateEventInformationCommand
            {
                Title = "AAA",
                Description = "BBB",
                Visible = false
            };
            var response = await HttpClient.PutAsync($"/events/{_event.Id}/information/{_eventInformation.Id}",
            new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));
           
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            RefreshContext();
            var eventInformation = Context.EventInformation.FirstOrDefault(s => s.Id == _event.Id);

            eventInformation.Title.Should().Be(command.Title);
            eventInformation.Description.Should().Be(command.Description);
            eventInformation.Visible.Should().Be(command.Visible);
        }

        private void FillData()
        {
            _event = new Event();

            Context.Events.Add(_event);
            _event.AddInformation("Title 1", "http://link.com", true);
            Context.SaveChanges();
            Context.SaveChanges();
            _eventInformation = _event.Information.First();

        }

    }
}