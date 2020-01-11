using NetBaires.Host;

namespace NetBaires.Api.Tests.Integration.Features.Me
{
    public class TwitterShould : IntegrationTestsBase
    {
        public TwitterShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

    }
}