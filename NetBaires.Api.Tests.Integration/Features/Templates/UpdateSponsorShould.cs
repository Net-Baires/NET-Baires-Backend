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
using NetBaires.Api.Features.Templates.UpdateTemplate;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Newtonsoft.Json;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Templates
{
    public class UpdateTemplateShould : IntegrationTestsBase
    {
        private Template _template;

        public UpdateTemplateShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
            GenerateData();
        }
        [Fact]
        public async Task Update_Template()
        {
            var newTemplateCommand = Fixture.Create<UpdateTemplateCommand>();
            var response = await HttpClient.PutAsync($"/templates/{_template.Id}",
                new StringContent(JsonConvert.SerializeObject(newTemplateCommand), Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            RefreshContext();
            var template = Context.Templates.Where(x=> x.Id == _template.Id).FirstOrDefault();

            template.Name.Should().Be(newTemplateCommand.Name);
            template.Description.Should().Be(newTemplateCommand.Description);
            template.TemplateContent.Should().Be(newTemplateCommand.TemplateContent);
        }
        private void GenerateData()
        {
            _template = new Template
            {
                Description = "Testt",
                Type = TemplateTypeEnum.EmailTemplateThanksAttended,
                Name = "Name of template",
                TemplateContent = "Html"
            };
            Context.Templates.Add(_template);
            Context.SaveChanges();
        }
    }
}