using System;
using Microsoft.EntityFrameworkCore;
using QuickStart.Application.Common.Handlers;

namespace QuickStart.Application.Features.Role;

public class GetRoleById( IHandlerContext context) : QueryHandler<GetRoleByIdRequest>(context)
{

    public override async Task<OperationResult> HandleAsync(GetRoleByIdRequest request,
     CancellationToken cancellationToken = default)
    {
        var existingRole = await context.DBRead.Roles.SingleOrDefaultAsync(
            r => r.Id == request.Id && r.Status, cancellationToken);

        if (existingRole == null) return SuccessResult.Create<GetRoleByIdResponse>(null);

        var response = new GetRoleByIdResponse
        {
            Role = existingRole
        };

        if (!request.IncludeRights) return SuccessResult.Create(response);

        response.Rights = await context.DBRead.RoleRights.Where(
            rr => rr.RoleId == existingRole.Id
        ).ToArrayAsync(cancellationToken);
        return SuccessResult.Create(response);
    }
}
