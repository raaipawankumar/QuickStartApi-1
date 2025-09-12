using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuickStart.Application.Common.Audit;
using QuickStart.Application.Common.Handlers;
using QuickStart.Application.Data.Extensions;
using QuickStart.Application.Features.Right;

namespace QuickStart.Application.Features.Role;

public class AddOrUpdateRole(IHandlerContext context, IAuditContext auditContext) 
: CommandHandler<AddOrUpdateRoleRequest>(context, auditContext)
{
    private Role? existingRole;
    public async override Task<OperationResult> HandleAsync(AddOrUpdateRoleRequest request, CancellationToken cancellationToken = default)
    {

        existingRole = await context.DBWrite.Roles.SingleOrDefaultAsync(
            r => r.Id == request.Id, cancellationToken);

        if (existingRole is null)
        {
            var roleId = await AddRoleAsync(request, cancellationToken);
            return SuccessResult.Create(roleId);
        }

        await UpdateRoleAsync(request, cancellationToken);
        return SuccessResult.Create(existingRole.Id);
        
    }

    private async Task UpdateRoleAsync(AddOrUpdateRoleRequest request, CancellationToken cancellationToken)
    {
        using var transaction = context.DBWrite.Database.BeginTransaction();
        try
        {
            existingRole!.Name = request.Name;
            existingRole.Code = request.Code;
            existingRole.Status = true;
            context.DBWrite.Roles.Update(existingRole);
            await context.DBWrite.SaveChangesAsync(cancellationToken);
            await AddUpdateRightsForRole(request, existingRole, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (SqlException)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    private async Task AddUpdateRightsForRole(AddOrUpdateRoleRequest request,
    Role existingRole,
     CancellationToken cancellationToken)
    {
        var newRightIds = request.Rights.Select(ri => ri) ;
            var existingRights = await context.DBWrite.RoleRights.Where(
                rr => rr.RoleId == existingRole.Id)
                .ToListAsync(cancellationToken);

            foreach (var existingRight in existingRights)
            {
                existingRight.IsRemoved = true;
            }

            foreach (var rightId in newRightIds)
            {
                var existing = existingRights.FirstOrDefault(rr => rr.RightId == rightId);
                if (existing is null)
                {
                    await context.DBWrite.RoleRights.AddAsync(new RoleRight
                    {
                        RoleId = existingRole.Id,
                        RightId = rightId,
                        IsRemoved = false
                    }, cancellationToken);
                }
                else
                {
                    existing.IsRemoved = false;
               }

            }
            await context.DBWrite.SaveChangesAsync(cancellationToken);
    }

    private async Task<int> AddRoleAsync(AddOrUpdateRoleRequest request, CancellationToken cancellationToken)
    {
        using var transaction = context.DBWrite.Database.BeginTransaction();
        try
        {
            Role newRole = new()
            {
                Name = request.Name,
                Code = request.Code,
                Status = true,

            };
            await context.DBWrite.Roles.AddAsync(newRole, cancellationToken);

            var roleRigts = request.Rights.Select(id => new RoleRight
            {
                RoleId = newRole.Id,
                RightId = id
            });
            await context.DBWrite.RoleRights.AddRangeAsync(roleRigts, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return newRole.Id;
        }
        catch (SqlException)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    protected override void SetAuditDetail(AddOrUpdateRoleRequest request)
    {
        var roleTable = context.DBRead.GetTableName<Role>();
        var roleRigtsTable = context.DBRead.GetTableName<RoleRight>();
        var affectedEntities = AuditAffectedEntity.Instance
        .Add(roleTable, string.Empty)
        .Add(roleRigtsTable, string.Join(",", request.Rights))
        .ToString();
        
        auditContext.EntityDetail = new EntityAuditDetail
        {
            Action = nameof(AddOrUpdateRole),
            NewValues = JsonSerializer.Serialize(request),
            OldValues = "{}",
            AffectedEntities = affectedEntities
        };
    }
}
