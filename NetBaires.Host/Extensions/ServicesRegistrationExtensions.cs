using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NetBaires.Api.Auth;
using NetBaires.Api.Services;
using NetBaires.Api.Services.BadGr;
using NetBaires.Api.Services.EventBrite;
using NetBaires.Api.Services.Meetup;
using NetBaires.Api.Services.Sync;
using NetBaires.Api.Services.Sync.Process;

namespace NetBaires.Host.Extensions
{
    public static class ServicesRegistrationExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<ICurrentUserSignalR, CurrentUserSignalR>();
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
            services.AddScoped<IAttendanceService, AttendanceService>();

            return services;
        }
    }
}
