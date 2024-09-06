using Application.Common.Models;
using Application.Common;
using Microsoft.AspNetCore.Identity;
using Persistence.Data;
using FarmLand_RPG_CommonIdentityServer.Common.Http;
using ErrorOr;
using FarmLand_RPG_CommonIdentityServer.Common.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Jwt;
using System.Text;
using Microsoft.OpenApi.Models;


namespace FarmLand_RPG_CommonIdentityServer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWeb(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton(TimeProvider.System);
            services.AddControllers();
            services.AddIdentity(configuration);

            services.AddProblemDeatails();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        []
                    }
                });
            });

            services.AddMappings();

            return services;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services, ConfigurationManager configuration)
        {
            JwtSettings settings = new();
            configuration.Bind(JwtSettings.SectionName, settings);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = settings.Issuer,
                        ValidAudience = settings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(settings.Secret))
                    };
                });

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password = new PasswordOptions
                {
                    RequireDigit = false,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false,
                    RequiredUniqueChars = 0,
                    RequireLowercase = false,
                };
            })
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddUserValidator<ApplicationUserValidator>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddApiEndpoints();

            services.AddAuthorizationBuilder();

            return services;
        }

        public static IServiceCollection AddProblemDeatails(this IServiceCollection services)
        {
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = (context) =>
                {
                    var httpContext = context.HttpContext;
                    var errors = httpContext?.Items[HttpContextItemKeys.Errors] as List<Error>;
                    if (errors != null)
                    {
                        context.ProblemDetails.Extensions.Add("errorCodes", errors.Select(e => e.Code));
                    }
                };
            });
            return services;
        }
    }
}
