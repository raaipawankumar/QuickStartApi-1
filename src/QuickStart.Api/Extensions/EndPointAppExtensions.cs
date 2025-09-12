using Asp.Versioning;
using QuickStart.Api.Common.Endpoints;

namespace QuickStart.Api.Extensions;

public static class EndPointAppExtensions
{
  public static WebApplication MapApplicationEndpoints(this WebApplication app)
  {
    var apiVersionSet = app.NewApiVersionSet()
      .HasApiVersion(new ApiVersion(1, 0))
      .HasApiVersion(new ApiVersion(2, 0))
      .ReportApiVersions()
      .Build();

    var routeGroupBuilder = app.MapGroup("/api/v{version:apiVersion}")
      .WithApiVersionSet(apiVersionSet);

    app.MapEndpoints(routeGroupBuilder);
    return app;
  }
}
