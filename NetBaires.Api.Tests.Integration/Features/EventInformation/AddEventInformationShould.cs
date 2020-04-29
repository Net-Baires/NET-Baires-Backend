using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.EventInformation.AddEventInformation;
using NetBaires.Api.Features.Materials.AddMaterial;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Newtonsoft.Json;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.EventInformation
{
    public class AddEventInformationShould : IntegrationTestsBase
    {
        private Event _event;

        public AddEventInformationShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }


        [Fact]
        public async Task Add_Information_To_The_Event()
        {
            FillData();

            var command = new AddEventInformationCommand
            {
                Title = "Test",
                Description = "Test",
                Visible = true
            };
            var response = await HttpClient.PostAsync($"/events/{_event.Id}/information",
            new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));
            await HttpClient.PostAsync($"/events/{_event.Id}/information",
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            RefreshContext();
            _event = Context.Events.Include(x => x.Information).FirstOrDefault(s => s.Id == _event.Id);

            _event.Information.Count.Should().Be(2);
            _event.Information.First().Title.Should().Be(command.Title);
            _event.Information.First().Description.Should().Be(command.Description);
            _event.Information.First().Visible.Should().Be(command.Visible);
        }

        private void FillData()
        {
            _event = new Event();

            Context.Events.Add(_event);
            Context.SaveChanges();
        }

    }
}