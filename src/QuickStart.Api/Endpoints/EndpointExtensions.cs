using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace QuickStart.Api.Common.Endpoints;

public static class EndpointExtensions
{
  public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
  {
    var endpointServiceDescriptors = assembly.DefinedTypes
      .Where(type => type is { IsAbstract: false, IsInterface: false }
       && type.IsAssignableTo(typeof(IEndpoint)))
      .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
      .ToArray();

    services.TryAddEnumerable(endpointServiceDescriptors);

    return services;
  }
  public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
  {
    var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
    IEndpointRouteBuilder routeBuilder = routeGroupBuilder is null ?
        app : routeGroupBuilder;

    foreach (var endpoint in endpoints)
    {
      endpoint.Map(routeBuilder);
    }

    return app;
  }
}
