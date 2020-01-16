using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBaires.Api.Auth;

namespace NetBaires.Host.Extensions
{
    public static class CacheRegistrationExtensions
    {
        public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration Configuration)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            services.AddResponseCaching(options =>
            {
                options.MaximumBodySize = 1024;
                options.UseCaseSensitivePaths = true;
            });

            return services;
        }
        public static IApplicationBuilder UseCache(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCaching();
            return app;
        }
    }
}
