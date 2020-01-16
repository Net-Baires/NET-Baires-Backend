using System;
using CacheManager.Core;
using EFSecondLevelCache.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBaires.Api.Auth;
using Newtonsoft.Json;

namespace NetBaires.Host.Extensions
{
    public static class EFCacheRegistrationExtensions
    {
        public static IServiceCollection AddCacheEF(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddEFSecondLevelCache();
            var jss = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            // Add an in-memory cache service provider
            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
            services.AddSingleton(typeof(ICacheManagerConfiguration),
                new CacheManager.Core.ConfigurationBuilder()
                    .WithJsonSerializer(serializationSettings: jss, deserializationSettings: jss)
                    .WithMicrosoftMemoryCacheHandle(instanceName: "MemoryCache1")
                    .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(10))
                    .Build());

            return services;
        }
        public static IApplicationBuilder UseCacheEf(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCaching();
            return app;
        }
    }
}
