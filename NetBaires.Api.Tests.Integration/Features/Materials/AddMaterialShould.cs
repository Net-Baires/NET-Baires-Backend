using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Materials.AddMaterial;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Newtonsoft.Json;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Materials
{
    public class AddMaterialShould : IntegrationTestsBase
    {
        private Event _event;

        public AddMaterialShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }


        [Fact]
        public async Task Add_Material()
        {
            FillData();

            var command = new AddMaterialCommand
            {
                Title = "Test",
                Link = "http://test.com.ar"
            };
            var response = await HttpClient.PostAsync($"/events/{_event.Id}/materials",
            new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));
            await HttpClient.PostAsync($"/events/{_event.Id}/materials",
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            RefreshContext();
            _event = Context.Events.Include(x => x.Materials).FirstOrDefault(s => s.Id == _event.Id);

            _event.Materials.Count.Should().Be(2);
            _event.Materials.First().Title.Should().Be(command.Title);
            _event.Materials.First().Link.Should().Be(command.Link);
        }

        private void FillData()
        {
            _event = new Event();

            Context.Events.Add(_event);
            Context.SaveChanges();
        }

    }
}