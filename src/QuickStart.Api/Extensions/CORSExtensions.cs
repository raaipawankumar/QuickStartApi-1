using System;

namespace QuickStart.Api.Extensions;

public static class CORSExtensions
{
    public const string DefaultPolicyName = "default";
    public static IServiceCollection AddConfigureCors(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: DefaultPolicyName, builder =>
            {
                var origins = configuration.GetValue<string>("AllowedHosts") ?? "*";
                builder.WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
        return services;
    }
}
