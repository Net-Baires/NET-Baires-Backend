using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetBaires.Api.Auth;
using NetBaires.Api.Controllers;
using NetBaires.Api.Options;
using NetBaires.Api.Services.BadGr;
using NetBaires.Api.Services.EventBrite;
using NetBaires.Api.Services.Meetup;
using NetBaires.Api.Services.Sync;
using NetBaires.Data;
using Newtonsoft.Json.Serialization;

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
            services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionActionFilter>();

            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }); ;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NET-Baires API", Version = "v1" });
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = "Authorization"
                    });
                c.EnableAnnotations();
            });
            services.AddMediatR(typeof(Startup));
            services.Configure<MeetupEndPointOptions>(Configuration.GetSection("MeetupEndPoint"));
            services.Configure<TwitterApiOptions>(Configuration.GetSection("TwitterApi"));
            services.Configure<EventBriteApiOptions>(Configuration.GetSection("EventBriteApi"));
            services.Configure<SlackEndPointOptions>(Configuration.GetSection("SlackEndPoint"));
            services.Configure<AssistanceOptions>(Configuration.GetSection("Assistance"));
            services.Configure<BadgrOptions>(Configuration.GetSection("Badgr"));

            services.AddHttpClient("");
            services.AddDbContext<NetBairesContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("NetBairesContext")));

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            services.AddResponseCaching(options =>
            {
                options.MaximumBodySize = 1024;
                options.UseCaseSensitivePaths = true;
            });
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IBadGrServices, BadGrServices>();
            services.AddScoped<IMeetupServices, MeetupServices>();
            services.AddScoped<IEventBriteServices, EventBriteServices>();
            services.AddScoped<ISyncServices, SyncServices>();


            
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseResponseCaching();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
