using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NetBaires.Api
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
                .AddInfraestructure()
                .AddOptions(Configuration)
                .AddAuthentication(Configuration)
                .AddCache(Configuration)
                .AddContext(Configuration)
                .AddSwagger()
                .AddServices()
                .AddTelemetry();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger(env)
                .UseCors()
                .UseAuthentication(env)
                .UseCache(env)
                .UseInfraestructure(env);
        }
    }
}
