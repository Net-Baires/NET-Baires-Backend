using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBaires.Host.Extensions;

namespace NetBaires.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCacheEF(Configuration)
                .AddInfraestructure()
                .AddOptions(Configuration)
                .AddAuthentication(Configuration)
                .AddCache(Configuration)
                .AddContext(Configuration)
                .AddSwagger()
                .AddServices()
                .AddSignalRServices()
                .AddTelemetry();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CorsRegistrationExtensions.UseCors(app.UseSwagger(env), Configuration)
                .UseAuthentication(env)
                .UseCache(env)
                .UseSignalRServices(env)
                .UseInfraestructure(env);
        }
    }
}
