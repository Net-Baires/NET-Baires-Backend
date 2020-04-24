using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using NetBaires.Api.ViewModels;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Templates
{
    public class GetTemplatesShould : IntegrationTestsBase
    {
        private Template _template;

        public GetTemplatesShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
            GenerateData();


        }
        [Fact]
        public async Task Return_All_Templates()
        {
            var response = await HttpClient.GetAsync("/templates");
            var templates = await response.Content.ReadAsAsync<List<TemplateViewModel>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            templates.Count.Should().Be(3);

            templates.Any(x => x.Name == _template.Name).Should().BeTrue();
            templates.Any(x => x.TemplateContent== _template.TemplateContent).Should().BeTrue();
            templates.Any(x => x.Description == _template.Description).Should().BeTrue();
            templates.Any(x => x.Type== _template.Type).Should().BeTrue();
            templates.Any(x => x.Id == _template.Id).Should().BeTrue();
        }

        [Fact]
        public async Task Return_One_Template()
        {
            var response = await HttpClient.GetAsync($"/templates/{_template.Id}");
            var templateReturn = await response.Content.ReadAsAsync<TemplateViewModel>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            templateReturn.Name.Should().Be(_template.Name);
            templateReturn.Description.Should().Be(_template.Description);
            templateReturn.Type.Should().Be(_template.Type);
            templateReturn.TemplateContent.Should().Be(_template.TemplateContent);
            templateReturn.Id.Should().Be(_template.Id);
        }

        private void GenerateData()
        {
            Context.Templates.Add(new Template());
            Context.Templates.Add(new Template());
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