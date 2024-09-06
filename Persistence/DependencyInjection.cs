using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfigurationManager configuration)
        {

            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseMySql(configuration["ConnectionString"], new MySqlServerVersion(ServerVersion.Parse("9.0")));
            });

            return services;
        }
    }
}
