using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.EventInformation
{
    public class RemoveEventInformationShould: IntegrationTestsBase
    {
        private Event _event;
        private Data.Entities.EventInformation _eventInformation;

        public RemoveEventInformationShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Remove_EventInformation()
        {
            FillData();

            var response = await HttpClient.DeleteAsync($"/events/{_event.Id}/information/{_eventInformation.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            RefreshContext();

            _event = Context.Events.Include(x => x.Information).FirstOrDefault(s => s.Id == _event.Id);
            _event.Information.Count.Should().Be(0);
        }


        private void FillData()
        {
            _event = new Event();
            _event.AddInformation("Title 1", "http://link.com",true);
            Context.Events.Add(_event);
            Context.SaveChanges();
            _eventInformation = _event.Information.First();
        }
    }
}