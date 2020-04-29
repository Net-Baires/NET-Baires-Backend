using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using NetBaires.Api.Features.Sponsors.NewSponsor;
using NetBaires.Api.Features.Templates.NewTemplate;
using NetBaires.Host;
using Newtonsoft.Json;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Templates
{
    public class NewTemplateShould : IntegrationTestsBase
    {
        public NewTemplateShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }
        [Fact]
        public async Task Add_New_Template()
        {
            var newTemplateCommand = Fixture.Create<NewTemplateCommand>();
            var response = await HttpClient.PostAsync("/templates", 
                new StringContent(JsonConvert.SerializeObject(newTemplateCommand), Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var template = Context.Templates.FirstOrDefault();

            template.Name.Should().Be(newTemplateCommand.Name);
            template.Description.Should().Be(newTemplateCommand.Description);
            template.Type.Should().Be(newTemplateCommand.Type);
            template.TemplateContent.Should().Be(newTemplateCommand.TemplateContent);
        }

    }
}