using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Events.AddMaterial;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Newtonsoft.Json;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Materials
{
    public class RemoveMaterial : IntegrationTestsBase
    {
        private Event _event;
        private Material _material;

        public RemoveMaterial(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Remove_Material()
        {
            FillData();

            var response = await HttpClient.DeleteAsync($"/events/{_event.Id}/materials/{_material.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            RefreshContext();

            _event = Context.Events.Include(x => x.Materials).FirstOrDefault(s => s.Id == _event.Id);
            _event.Materials.Count.Should().Be(0);
        }


        private void FillData()
        {
            _event = new Event();
            _event.AddMaterial("Title 1", "http://link.com");
            Context.Events.Add(_event);
            Context.SaveChanges();
            _material = _event.Materials.First();
        }
    }
}