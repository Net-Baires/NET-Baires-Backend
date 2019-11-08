﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBaires.Api.Options;

namespace NetBaires.Api
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
            services.Configure<ConnectionStringsOptions>(Configuration.GetSection("ConnectionStrings"));
            return services;
        }
    }
}
