using Microsoft.EntityFrameworkCore;
using QuickStart.Application.Common.Handlers;

namespace QuickStart.Application.Features.Role;

public class GetRolesWithSummary(IHandlerContext context) : QueryHandler<PagedInput>(context)
{
    public override async Task<OperationResult> HandleAsync(PagedInput request, CancellationToken cancellationToken = default)
    {
        var skip = request.PageNo - 1;
        var query = (from role in context.DBRead.Roles.AsNoTracking()
                    orderby role.LastUpdated descending
                    
                    select new GetRolesWithSummaryResponse
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        TotalRights = context.DBRead.RoleRights.AsNoTracking()
                           .Where(rr => rr.RoleId == role.Id && !rr.IsRemoved).Count(),
                        TotalUsers = context.DBRead.UserRoles.AsNoTracking()
                           .Where(ur => ur.RoleId == role.Id && ur.IsActive).Count(),
                        LastUpdated = role.LastUpdated
                    }).Skip(skip).Take(request.PageSize);
         
        var roles = await query.ToListAsync(cancellationToken);
        return new SuccessResult<IList<GetRolesWithSummaryResponse>>(roles);
    }
}
