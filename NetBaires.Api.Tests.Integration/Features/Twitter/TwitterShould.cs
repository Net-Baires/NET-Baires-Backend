using NetBaires.Host;

namespace NetBaires.Api.Tests.Integration.Features.Twitter
{
    public class TwitterShould : IntegrationTestsBase
    {
        public TwitterShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

    }
}