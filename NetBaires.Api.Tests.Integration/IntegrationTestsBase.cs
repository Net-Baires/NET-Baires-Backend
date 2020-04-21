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
using NetBaires.Api.Services.Sync;
using System.Linq;
using NetBaires.Host;

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
        protected ISyncServices SyncServices;
        protected IAttendanceService AttendanceService;
        protected NetBairesContext Context;
        protected IQueueServices QueueServices;
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

                    AttendanceService = scopedServices
                    .GetRequiredService<IAttendanceService>();

                    FileServices = scopedServices
                        .GetRequiredService<IFilesServices>();

                    QueueServices = scopedServices
                        .GetRequiredService<IQueueServices>();

                    SyncServices = scopedServices
                          .GetRequiredService<ISyncServices>();
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
            CleanAuthorizationHeader();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Token}");
        }

        private void CleanAuthorizationHeader()
        {
            if (HttpClient.DefaultRequestHeaders.Contains("Authorization"))
                HttpClient.DefaultRequestHeaders.Remove("Authorization");
        }

        protected async Task AuthenticateAdminAsync()
        {
            var token = await UserService.AuthenticateOrCreate("admin@admin.com");

            CleanAuthorizationHeader();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Token}");
        }
        protected void RefreshContext()
        {
            var refreshableObjects = Context.ChangeTracker.Entries().Select(c => c.Entity).ToList();
            foreach (var item in refreshableObjects)
            {
                Context.Entry(item).Reload();
            }
        }

    }
}