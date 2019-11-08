using System.Text;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetBaires.Api.Auth;
using NetBaires.Api.Filters;
using NetBaires.Api.Handlers.Badges;
using NetBaires.Api.Handlers.Badges.NewBadge;
using NetBaires.Api.Options;
using NetBaires.Api.Services;
using NetBaires.Api.Services.BadGr;
using NetBaires.Api.Services.EventBrite;
using NetBaires.Api.Services.Meetup;
using NetBaires.Api.Services.Sync;
using NetBaires.Api.Services.Sync.Process;
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
                options.RespectBrowserAcceptHeader = true;
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<NewBadgeValidator>());
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
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(typeof(Startup));
            services.Configure<MeetupEndPointOptions>(Configuration.GetSection("MeetupEndPoint"));
            services.Configure<TwitterApiOptions>(Configuration.GetSection("TwitterApi"));
            services.Configure<EventBriteApiOptions>(Configuration.GetSection("EventBriteApi"));
            services.Configure<SlackEndPointOptions>(Configuration.GetSection("SlackEndPoint"));
            services.Configure<AttendanceOptions>(Configuration.GetSection("Attendance"));
            services.Configure<BadgrOptions>(Configuration.GetSection("Badgr"));
            services.Configure<CommonOptions>(Configuration.GetSection("Common"));
            services.Configure<BadgesOptions>(Configuration.GetSection("Badges"));
            services.Configure<ConnectionStringsOptions>(Configuration.GetSection("ConnectionStrings"));
            services.AddHttpClient("");
            services.AddDbContext<NetBairesContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("NetBairesContext"), b => b.MigrationsAssembly("NetBaires.Data")));

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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IBadGrServices, BadGrServices>();
            services.AddScoped<IMeetupServices, MeetupServices>();
            services.AddScoped<IEventBriteServices, EventBriteServices>();
            services.AddScoped<ISyncServices, SyncServices>();
            services.AddScoped<IFilesServices, FilesServices>();
            services.AddScoped<IBadgesServices, BadgesServices>();
            services.AddScoped<IExternalsSyncServices, EventBriteSyncServices>();
            services.AddScoped<IExternalsSyncServices, MeetupSyncServices>();
            services.AddScoped<IProcessEvents, ProcessEventsFromEventbrite>();
            services.AddScoped<IProcessEvents, ProcessEventsFromMeetup>();
            services.AddApplicationInsightsTelemetry();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseResponseCaching();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{UserAnonymous.Anonymous.ToString()}/swagger.json", "NET-Baires Api - Anonymous");
                c.SwaggerEndpoint($"/swagger/{UserRole.Admin}/swagger.json", "NET-Baires Api - Admin");
                c.SwaggerEndpoint($"/swagger/{UserRole.Member}/swagger.json", "NET-Baires Api - Miembro");
                c.SwaggerEndpoint($"/swagger/{UserRole.Organizer}/swagger.json", "NET-Baires Api - Organizador");
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
