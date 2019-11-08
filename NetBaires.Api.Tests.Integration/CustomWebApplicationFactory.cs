﻿using Microsoft.AspNetCore.Hosting;
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
using NetBaires.Api.Services.Meetup;
using NetBaires.Api.Tests.Integration.Services;

namespace NetBaires.Api.Tests.Integration
{

    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(async services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<NetBairesContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<NetBairesContext>((options, context) =>
                {
                    context.UseInMemoryDatabase("InMemoryDbForTesting");

                });
                services.AddTransient<IEventBriteServices, EventBriteServicesDummy>();
                services.AddTransient<IMeetupServices, MeetupServicesDummy>();
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<NetBairesContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                    try
                    {
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