using Mapster;

namespace FarmLand_RPG_CommonIdentityServer.Common.Mappings
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(AssemblyReference).Assembly);

            services.AddSingleton(config);
            services.AddMapster();
            return services;
        }
    }
}
