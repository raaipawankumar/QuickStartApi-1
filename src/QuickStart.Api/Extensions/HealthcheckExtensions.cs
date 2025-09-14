using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
namespace QuickStart.Api.Extensions;

public static class HealthcheckExtensions
{
  public static IServiceCollection AddHealthCheckWithConfiguration(this IServiceCollection services, ConfigurationManager configuration)
  {
    services.AddHealthChecks()
    .AddSqlServer(
      name: "SQL Server",
      connectionString: configuration.GetConnectionString("smartwx_connection_string"),
      healthQuery: "SELECT 1;",
      failureStatus: HealthStatus.Unhealthy
    );
    return services;
  }
  public static IEndpointRouteBuilder MapHealthCheckWithConfiguration(this IEndpointRouteBuilder endpoints)
  {
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
      Predicate = (_) => true,
      ResponseWriter = (context, report) =>
        {
          context.Response.ContentType = "application/json";
          return context.Response.WriteAsJsonAsync(report);
        }
    });
    return endpoints;
  }
}
