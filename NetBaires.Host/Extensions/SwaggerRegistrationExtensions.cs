using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Host.Extensions
{
    public static class SwaggerRegistrationExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(UserRole.Member.ToString(), new OpenApiInfo { Title = "NET-Baires Api - Miembro", Version = UserRole.Member.ToString() });
                c.SwaggerDoc(UserRole.Admin.ToString(), new OpenApiInfo { Title = "NET-Baires Api - Admin", Version = UserRole.Admin.ToString() });
                c.SwaggerDoc(UserRole.Organizer.ToString(), new OpenApiInfo { Title = "NET-Baires Api - Organizador", Version = UserRole.Organizer.ToString() });
                c.SwaggerDoc(UserAnonymous.Anonymous.ToString(), new OpenApiInfo { Title = "NET-Baires Api - Anonymous", Version = UserAnonymous.Anonymous.ToString() });
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = "Authorization"
                    });
                c.EnableAnnotations();
            });
            return services;
        }
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{UserAnonymous.Anonymous.ToString()}/swagger.json", "NET-Baires Api - Anonymous");
                c.SwaggerEndpoint($"/swagger/{UserRole.Admin}/swagger.json", "NET-Baires Api - Admin");
                c.SwaggerEndpoint($"/swagger/{UserRole.Member}/swagger.json", "NET-Baires Api - Miembro");
                c.SwaggerEndpoint($"/swagger/{UserRole.Organizer}/swagger.json", "NET-Baires Api - Organizador");
            });
            app.UseSwagger();
            return app;
        }

    }
}
