using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBaires.Data;

namespace NetBaires.Host.Extensions
{
    public static class EntityFrameworkRegistrationExtensions
    {
        public static IServiceCollection AddContext(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<NetBairesContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("NetBairesContext"), b => b.MigrationsAssembly("NetBaires.Data")));
            return services;
        }
    }
}
