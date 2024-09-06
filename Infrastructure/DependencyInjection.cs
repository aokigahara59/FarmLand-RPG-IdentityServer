using Application.Common.Interfaces;
using Infrastructure.Jwt;
using Infrastructure.RefreshToken;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<JwtSettings>(options => configuration.Bind(JwtSettings.SectionName, options));

            services.Configure<RefreshTokenSettings>(options => configuration.Bind(RefreshTokenSettings.SectionName, options));

            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            return services;
        }
    }
}
