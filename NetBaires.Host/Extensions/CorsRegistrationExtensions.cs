using Microsoft.AspNetCore.Builder;

namespace NetBaires.Api
{
    public static class CorsRegistrationExtensions
    {
       
        public static IApplicationBuilder UseCors(this IApplicationBuilder app)
        {
            app.UseCors(x => x
                              .AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader());
            return app;
        }

    }
}
