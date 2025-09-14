using System;
using Asp.Versioning;

namespace QuickStart.Api.Extensions;

public static class ApiVesioningExtensions
{
    const string ApiVersionQueryStringKey = "api-version";
    const string ApiVersionHeaderKey = "X-Api-Version";
    public static IServiceCollection AddVersioningWithConfiguration(
        this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new QueryStringApiVersionReader(ApiVersionQueryStringKey),
                new HeaderApiVersionReader(ApiVersionHeaderKey)
            );

        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        return services;
    }

}
