using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using QuickStart.Api.Common.Endpoints;
using QuickStart.Application.Common.Cache;
using QuickStart.Application.Common.Handlers;
using QuickStart.Application.Features.General;
using QuickStart.Application.Features.GeneralSettings;
using QuickStart.Application.Features.Language;

namespace QuickStart.Api.Endpoints;

public class SettingsEndpoints : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        var apiVersion1 = new ApiVersion(1, 0);
        var group = app.MapGroup("/Settings");
        group.MapGet("/basic", GetGeneralSettings)
            .MapToApiVersion(apiVersion1);

        group.MapGet("/languages", GetAllLanguages)
        .MapToApiVersion(apiVersion1);
    }



    private async Task<IResult> GetGeneralSettings(
       [FromServices] IHandlerContext context,
       [FromServices] ILogger<GetGeneralSettings> logger,
       CancellationToken cancellationToken)
    {
        var query = new GetGeneralSettings(context);
        var result = await HandlerBuilder.For(query, context, logger)
        .WithLogging(logger)
        .WithCaching<GeneralSetting>(CacheKeyBuilder.Build(AppCacheKey.AllGeneralSettings),
         AppCacheOptions.Default)
        .Build()
        .HandleAsync(NoInput.Instance, cancellationToken);

        return result.Match<IResult>(
            data => TypedResults.Ok(data),
            (code, errors) => TypedResults.BadRequest(errors));
    }
     private async Task<IResult> GetAllLanguages([FromServices] IHandlerContext context,
       [FromServices] ILogger<GetAllLanguages> logger,
       CancellationToken cancellationToken)
    {
       var query = new GetAllLanguages(context);
        var result = await HandlerBuilder.For(query, context, logger)
        .WithLogging(logger)
        .WithCaching<IEnumerable<GeneralSetting>>(CacheKeyBuilder.Build("settings", "languages-all"), AppCacheOptions.Default)
        .Build()
        .HandleAsync(NoInput.Instance, cancellationToken);

        return result.Match<IResult>(
            data => TypedResults.Ok(data),
            (code, errors) => TypedResults.BadRequest(errors));
    }
}
