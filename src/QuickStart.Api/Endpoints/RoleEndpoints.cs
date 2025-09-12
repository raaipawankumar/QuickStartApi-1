using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using QuickStart.Api.Common.Endpoints;
using QuickStart.Application.Common.Audit;
using QuickStart.Application.Common.Cache;
using QuickStart.Application.Common.Handlers;
using QuickStart.Application.Features.Language;
using QuickStart.Application.Features.Role;

namespace QuickStart.Api.Endpoints;

public class RoleEndpoints : IEndpoint
{
    
    public void Map(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/roles").WithTags("Identity");
        group.MapGet("/", GetAll).MapToApiVersion(new ApiVersion(1));
        group.MapGet("/{id}", Get).MapToApiVersion(new ApiVersion(1));
        group.MapPost("/", AddOrUpdate).MapToApiVersion(new ApiVersion(1));
        group.MapDelete("/{id}", Delete).MapToApiVersion(new ApiVersion(1));
    }
private async Task<IResult> GetAll(
        [FromServices] IHandlerContext context,
        [FromServices] ILogger<GetRolesWithSummary> logger)
    {
        var handler = new GetRolesWithSummary(context);

        var cacheKey = CacheKeyBuilder.Build(AppCacheKey.Role.AllRolesWithSummary);

        return (
            await HandlerBuilder.For(handler, context, logger)
            .WithCaching<IEnumerable<Language>>(cacheKey)
            .Build()
            .HandleAsync(PagedInput.Default))
            .Match<IResult>(
            data => TypedResults.Ok(data),
            (errorCode, errors) => TypedResults.NoContent());

    }
     private async Task<IResult> Get(
         [FromRoute] int id,
        [FromServices] IHandlerContext context,
         [FromServices] ILogger<GetAllLanguages> logger,
         [FromQuery] bool includeRights = false
         
    )
    {
        var handler = new GetRoleById(context);
        var request = new GetRoleByIdRequest
        {
            Id = id,
            IncludeRights = includeRights
        };
        return (await HandlerBuilder.For(handler, context, logger)
        .
          .Build()
          .HandleAsync(request))
          .Match<IResult>(
              role => role == null ? TypedResults.NotFound() : TypedResults.Ok(role),
              (errorCode, errors) => TypedResults.Empty
              );
    }
     private async Task<IResult> AddOrUpdate(
        [FromBody] AddOrUpdateRoleRequest request,
        [FromServices] IHandlerContext context,
        [FromServices] IAuditContext auditContext,
        [FromServices] ILogger<GetAllLanguages> logger
    )
    {
        var handler = new AddOrUpdateRole(context, auditContext);
        return (
            await HandlerBuilder.For(handler, context, logger)
            .WithValidator(new AddOrUpdateRoleValidator(context))
            .WithCacheInvalidation(() => [AppCacheKey.Language.AllLanguages])
            .WithAudit(auditContext)
            .Build()
            .HandleAsync(request)

        ).Match<IResult>(
            (data) => TypedResults.Ok(),
            (code, errors) => TypedResults.BadRequest(errors)
         );
    }
    private async Task<IResult> Delete([FromRoute] int id,
     [FromServices] IHandlerContext context,
     [FromServices] IAuditContext auditContext,
     [FromServices] ILogger<GetAllLanguages> logger
     )
    {
        var handler = new DeleteRole(context, auditContext);

        return (await HandlerBuilder.For(handler, context, logger)
        .WithCacheInvalidation(() => [AppCacheKey.Role.AllRolesWithSummary])
        .Build()
        .HandleAsync(id))
        .Match<IResult>(
              data => TypedResults.Ok(data),
              (errorCode, errors) =>
              {
                  if (errorCode == ErrorResultCode.NotFound) return TypedResults.NotFound();
                  if (errors.Count > 0) return TypedResults.BadRequest(errors);
                  return TypedResults.Empty;
              }
        );

    }

   

   

    
}
