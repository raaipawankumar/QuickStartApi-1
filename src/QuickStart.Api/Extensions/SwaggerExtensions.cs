using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using QuickStart.Api.Common.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace QuickStart.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddConfigureSwaggerGen(this IServiceCollection services)
    {
        services.ConfigureOptions<SwaggerConfiguration>();
        services.AddSwaggerGen();
        return services;
    }
    public static WebApplication UseConfigureSwaggerUI(this WebApplication app)
    {
        app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }

    });
    return app;
    }
}
