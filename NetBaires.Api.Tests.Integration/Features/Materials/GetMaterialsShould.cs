using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Materials.GetMaterials;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Newtonsoft.Json;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Materials
{
    public class GetMaterialsShould : IntegrationTestsBase
    {
        private Event _event;
        private Material _material;

        public GetMaterialsShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }


        [Fact]
        public async Task Get_All_Materials()
        {
            FillData();

            var response = await HttpClient.GetAsync($"/events/{_event.Id}/materials");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var materials = await response.Content.ReadAsAsync<List<MaterialViewModel>>();


            materials.Count.Should().Be(3);
            materials.Any(x => x.Id == _material.Id
                               &&
                               x.Title == _material.Title
                               &&
                               x.Link == _material.Link).Should().BeTrue();
        }

        private void FillData()
        {
            _event = new Event();
            _event.AddMaterial("Title 1", "http://link.com");
            _event.AddMaterial("Title 2", "http://link.com");
            _event.AddMaterial("Title 3", "http://link.com");
            Context.Events.Add(_event);
            Context.SaveChanges();
            _material = _event.Materials.First();
        }

    }
}