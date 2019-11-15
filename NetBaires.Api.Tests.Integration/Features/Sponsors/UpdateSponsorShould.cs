using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Handlers.Sponsors;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Sponsors
{
    public class UpdateSponsorShould : IntegrationTestsBase
    {
        public UpdateSponsorShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }
        [Fact]
        public async Task Update_Sponsor()
        {
            await AddNewSponsor();
            var sponsor = Context.Sponsors.FirstOrDefault();

            var file1 = File.OpenRead(@"Images\Blanco.png");
            var badgeFile = new StreamContent(file1);
            var formData = new MultipartFormDataContent();
            formData.Add(badgeFile, nameof(NewSponsorCommand.ImageFile), $"NewImage.jpg");
            formData.Add(new StringContent("New Name"), nameof(NewSponsorCommand.Name));
            formData.Add(new StringContent("New Description"), nameof(NewSponsorCommand.Description));
            formData.Add(new StringContent("New SiteUrl"), nameof(NewSponsorCommand.SiteUrl));

            var response = await HttpClient.PutAsync($"/sponsors/{sponsor.Id}", formData);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var oldLogoFileName = sponsor.LogoFileName;

            RefreshContext();
            sponsor = Context.Sponsors.FirstOrDefault();
            sponsor.Name.Should().Be("New Name");
            sponsor.Description.Should().Be("New Description");
            sponsor.SiteUrl.Should().Be("New SiteUrl");

            (await FileServices.GetAsync(oldLogoFileName, Api.Services.Container.Sponsors)).Should().BeNull();
            (await FileServices.GetAsync(sponsor.LogoFileName, Api.Services.Container.Sponsors)).Should().NotBeNull();
            await FileServices.DeleteAsync(sponsor.LogoFileName, Api.Services.Container.Sponsors);
        }

        private async Task<HttpResponseMessage> AddNewSponsor()
        {
            var file1 = File.OpenRead(@"Images\Blanco.png");
            var badgeFile = new StreamContent(file1);
            var formData = new MultipartFormDataContent();
            formData.Add(badgeFile, nameof(NewSponsorCommand.ImageFile), $"NewImage.jpg");
            formData.Add(new StringContent("Name"), nameof(NewSponsorCommand.Name));
            formData.Add(new StringContent("Description"), nameof(NewSponsorCommand.Description));
            formData.Add(new StringContent("SiteUrl"), nameof(NewSponsorCommand.SiteUrl));
            var response = await HttpClient.PostAsync("/sponsors", formData);
            return response;
        }
    }
}