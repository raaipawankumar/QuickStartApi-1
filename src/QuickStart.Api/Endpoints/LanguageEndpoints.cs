using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using QuickStart.Api.Common.Endpoints;
using QuickStart.Application.Common.Audit;
using QuickStart.Application.Common.Cache;
using QuickStart.Application.Common.Handlers;
using QuickStart.Application.Features.Language;

namespace QuickStart.Api.Endpoints;

public class LanguageEndpoints : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/languages");
        group.MapGet("/", GetAll).MapToApiVersion(new ApiVersion(1));
        group.MapGet("/{id}", Get).MapToApiVersion(new ApiVersion(1));
        group.MapPost("/", AddOrUpdate).MapToApiVersion(new ApiVersion(1));
        group.MapPut("/{id}", AddOrUpdate).MapToApiVersion(new ApiVersion(1));
        group.MapDelete("/{id}", Delete).MapToApiVersion(new ApiVersion(1));
    }

    private async Task<IResult> Delete([FromRoute] Guid id,
     [FromServices] IHandlerContext context,
     [FromServices] IAuditContext auditContext,
     [FromServices] ILogger<GetAllLanguages> logger
     )
    {
        var handler = new DeleteLanguage(context, auditContext);

        return (await HandlerBuilder.For(handler, context, logger)
        .WithCacheInvalidation(() => [AppCacheKey.Language.AllLanguages])
         .Build()
          .HandleAsync(id))
          .Match<IResult>(
              data => TypedResults.Ok(data),
              (errorCode, errors) =>
                  TypedResults.NotFound()
              );

    }

    private async Task<IResult> AddOrUpdate(
        [FromBody] AddUpdateLanguageRequest request,
        [FromServices] IHandlerContext context,
        [FromServices] IAuditContext auditContext,
        [FromServices] ILogger<GetAllLanguages> logger
    )
    {
        var handler = new AddUpdateLanguage(context, auditContext);
        return (
            await HandlerBuilder.For(handler, context, logger)
            .WithValidator(new AddUpdateLanguageValidator(context))
            .WithCacheInvalidation(() => [AppCacheKey.Language.AllLanguages])
            .WithAudit(auditContext)
            .Build()
            .HandleAsync(request)

        ).Match<IResult>(
            (data) => TypedResults.Ok(),
            (code, errors) => TypedResults.BadRequest(errors)
         );
    }

    private async Task<IResult> Get(
         [FromRoute] Guid id,
         [FromServices] IHandlerContext context,
         [FromServices] ILogger<GetAllLanguages> logger
         
    )
    {
        var handler = new GetLanguageByIdQuery(context);

        return (await HandlerBuilder.For(handler, context, logger)
          .Build()
          .HandleAsync(id))
          .Match<IResult>(
              data => TypedResults.Ok(data),
              (errorCode, errors) =>
                  TypedResults.NotFound()
              );
    }

    private async Task<IResult> GetAll(
        [FromServices] IHandlerContext context,
        [FromServices] ILogger<GetAllLanguages> logger)
    {
        var handler = new GetAllLanguages(context);

        var cacheKey = CacheKeyBuilder.Build();

        return (await HandlerBuilder.For(handler, context, logger)
        .WithCaching<IEnumerable<Language>>(cacheKey)
        .Build()
        .HandleAsync(NoInput.Instance))
        .Match<IResult>(
            data => TypedResults.Ok(data),
            (errorCode, errors) => TypedResults.NoContent());

    }
}
