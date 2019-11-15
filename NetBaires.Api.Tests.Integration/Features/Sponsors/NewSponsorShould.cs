using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.Features.Badges.NewBadge;
using NetBaires.Api.Handlers.Sponsors;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Sponsors
{
    public class NewSponsorShould : IntegrationTestsBase
    {
        public NewSponsorShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }
        [Fact]
        public async Task Add_New_Sponsor()
        {
            var file1 = File.OpenRead(@"Images\Blanco.png");
            var badgeFile = new StreamContent(file1);
            var formData = new MultipartFormDataContent();
            formData.Add(badgeFile, nameof(NewSponsorCommand.ImageFile), $"NewImage.jpg");
            formData.Add(new StringContent("Name"), nameof(NewSponsorCommand.Name));
            formData.Add(new StringContent("Description"), nameof(NewSponsorCommand.Description));
            formData.Add(new StringContent("SiteUrl"), nameof(NewSponsorCommand.SiteUrl));
            var response = await HttpClient.PostAsync("/sponsors", formData);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var sponsor = Context.Sponsors.FirstOrDefault();
            sponsor.Name.Should().Be("Name");
            sponsor.Description.Should().Be("Description");
            sponsor.SiteUrl.Should().Be("SiteUrl");
            (await FileServices.GetAsync(sponsor.LogoFileName, Api.Services.Container.Sponsors)).Should().NotBeNull();

            await FileServices.DeleteAsync(sponsor.LogoFileName, Api.Services.Container.Sponsors);
        }

    }
}