using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;
using Template = NetBaires.Data.Entities.Template;

namespace NetBaires.Api.Tests.Integration.Features.Templates
{
    public class DeleteTemplatesShould : IntegrationTestsBase
    {

        public DeleteTemplatesShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }
        [Fact]
        public async Task Delete_Template()
        {
            var newTemplate = new Template();
            Context.Templates.Add(newTemplate);
            Context.SaveChanges();
            var response = await HttpClient.DeleteAsync($"/templates/{newTemplate.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();
            Context.Sponsors.Count().Should().Be(0);
        }
        [Fact]
        public async Task Not_Delete_Template_Is_In_Event()
        {
            var newTemplate = new Template
            {
                Type = TemplateTypeEnum.EmailTemplateThanksSpeakers
            };
            var newEvent = new Event();
            newEvent.SetEmailTemplateThanksSpeakers(newTemplate);
            Context.Events.Add(newEvent);
            Context.SaveChanges();
            var response = await HttpClient.DeleteAsync($"/templates/{newTemplate.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }
    }
}