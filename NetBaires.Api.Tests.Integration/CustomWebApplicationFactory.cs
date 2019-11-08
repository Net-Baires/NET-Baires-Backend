using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NetBaires.Data;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Auth;
using NetBaires.Api.Services.EventBrite;
using NetBaires.Api.Services.EventBrite.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetBaires.Api.Tests.Integration
{

    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(async services =>
            {
                // Remove the app's ApplicationDbContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<NetBairesContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add ApplicationDbContext using an in-memory database for testing.
                services.AddDbContext<NetBairesContext>((options, context) =>
                {
                    context.UseInMemoryDatabase("InMemoryDbForTesting");

                });
                services.AddTransient<IEventBriteServices, EventBriteServicesDummy>();
                // Build the service provider.
                var sp = services.BuildServiceProvider();
                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<NetBairesContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
                    var a = scopedServices.GetRequiredService<IUserService>();
                    // Ensure the database is created.
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                    try
                    {
                        // Seed the database with test data.
                        UtilitiesDb.InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}