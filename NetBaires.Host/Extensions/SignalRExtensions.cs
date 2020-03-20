using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace NetBaires.Host.Extensions
{
    public static class SignalRExtensions
    {
        public static IServiceCollection AddSignalRServices(this IServiceCollection services)
        {
            services.AddSignalR();
            return services;
        }
        public static IApplicationBuilder UseSignalRServices(this IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            
            return app;
        }
    }
}