using Microsoft.Extensions.DependencyInjection;

namespace NetBaires.Host.Extensions
{
    public static class TelemetryRegistrationExtensions
    {
        public static IServiceCollection AddTelemetry(this IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            return services;
        }
    }
}
