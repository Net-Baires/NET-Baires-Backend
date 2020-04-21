using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Host.Extensions
{
    public static class OptionsRegistrationExtensions
    {
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<MeetupEndPointOptions>(Configuration.GetSection("MeetupEndPoint"));
            services.Configure<TwitterApiOptions>(Configuration.GetSection("TwitterApi"));
            
            services.Configure<EventBriteApiOptions>(Configuration.GetSection("EventBriteApi"));
            services.Configure<SlackEndPointOptions>(Configuration.GetSection("SlackEndPoint"));
            services.Configure<AttendanceOptions>(Configuration.GetSection("Attendance"));
            services.Configure<BadgrOptions>(Configuration.GetSection("Badgr"));
            services.Configure<CommonOptions>(Configuration.GetSection("Common"));
            services.Configure<BadgesOptions>(Configuration.GetSection("Badges"));
            services.Configure<CorsOptions>(Configuration.GetSection("Cors"));

            
            services.Configure<ConnectionStringsOptions>(Configuration.GetSection("ConnectionStrings"));
            return services;
        }
    }
}
