using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetBaires.Api.Features.Badges.DeleteBadge;
using NetBaires.Api.Features.Badges.NewBadge;
using NetBaires.Api.Filters;
using Newtonsoft.Json.Serialization;

namespace NetBaires.Api
{
    public static class InfraestructureRegistrationExtensions
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionActionFilter>();
                options.RespectBrowserAcceptHeader = true;
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            })
              .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<NewBadgeValidator>())
              .AddFeatureFolders();

            services.AddAutoMapper(typeof(DeleteBadgeHandler));
            services.AddMediatR(typeof(DeleteBadgeHandler));

            services.AddHttpClient("");
            return services;
        }
        public static IApplicationBuilder UseInfraestructure(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            return app;
        }
    }
}
