using Application.Common.Behaviours;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);

            services.AddMediatR(options =>
            {
                options.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

                options.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly);
            });

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(Application.AssemblyReference).Assembly);
            services.AddSingleton(config);

            return services;
        }
    }
}
