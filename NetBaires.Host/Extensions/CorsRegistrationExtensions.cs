using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using NetBaires.Api.Options;

namespace NetBaires.Host.Extensions
{
    public static class CorsRegistrationExtensions
    {
       
        public static IApplicationBuilder UseCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseCors(x => x
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowAnyOrigin()
                              .WithOrigins(configuration.GetSection("Cors").Get<CorsOptions>().Urls.ToArray())
                              .AllowCredentials());
            return app;
        }

    }
}
