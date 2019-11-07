using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using NetBaires.Api.Auth;
using System.Threading.Tasks;
using NetBaires.Data;
using NetBaires.Api.Services;

namespace NetBaires.Api.Tests.Integration
{
    public class IntegrationTestsBase :
      IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        protected readonly HttpClient HttpClient;
        private readonly CustomWebApplicationFactory<Startup>
            _factory;
        protected IUserService UserService;

        protected IFilesServices FileServices;

        protected NetBairesContext Context;
        public IntegrationTestsBase(
            CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            HttpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(async services =>
                {
                    var serviceProvider = services.BuildServiceProvider();

                    var scope = serviceProvider.CreateScope();

                    var scopedServices = scope.ServiceProvider;
                    UserService = scopedServices
                        .GetRequiredService<IUserService>();
                    FileServices = scopedServices
                          .GetRequiredService<IFilesServices>();
                    Context = scopedServices
                        .GetRequiredService<NetBairesContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<IntegrationTestsBase>>();

                    try
                    {
                        //UtilitiesDb.ReinitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding " +
                            "the database with test messages. Error: {Message}",
                            ex.Message);
                    }
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
        protected async Task AuthenticateAsync(string email)
        {
            var token = await UserService.AuthenticateOrCreate(email);
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
        protected async Task AuthenticateAdminAsync()
        {
            var token = await UserService.AuthenticateOrCreate("admin@admin.com");
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Token}");
        }

    }
}