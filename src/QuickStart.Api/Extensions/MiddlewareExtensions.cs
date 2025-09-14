using QuickStart.Api.Middlewares;

namespace QuickStart.Api.Extensions;

public static class MiddlewareExtensions
{
  public static IServiceCollection AddMiddlewaresWithConfiguration(this IServiceCollection services, IConfigurationManager configuration)
  {
    services.AddScoped<ExceptionHandlingMiddleware>();
    services.AddScoped<AddCorreltationIdMiddleware>();
    services.Configure<SecurityOptions>(
      configuration.GetSection(SecurityOptions.SectionName))
    .AddScoped<SecurityMiddleware>();

    return services;
  }
  public static IServiceCollection AddSecurityMiddleware(this IServiceCollection services,
     ConfigurationManager configuration)
  {


    return services;
  }

}
