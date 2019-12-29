using Microsoft.AspNetCore.Builder;

namespace NetBaires.Host.Extensions
{
    public static class CorsRegistrationExtensions
    {
       
        public static IApplicationBuilder UseCors(this IApplicationBuilder app)
        {
            app.UseCors(x => x
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowAnyOrigin()
                              .WithOrigins("http://localhost:8080")
                              .AllowCredentials());
            return app;
        }

    }
}
