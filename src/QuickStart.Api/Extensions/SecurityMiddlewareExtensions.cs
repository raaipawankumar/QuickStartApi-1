using System;
using QuickStart.Api.Middlewares;

namespace QuickStart.Api.Extensions;

public static class SecurityMiddlewareExtensions
{
    public static IServiceCollection AddSecurityMiddleware(this IServiceCollection services,
     ConfigurationManager configuration)
    {
        services.Configure<SecurityOptions>(configuration.GetSection(SecurityOptions.SectionName))
         .AddScoped<SecurityMiddleware>();
         
        return services;
    }

}
