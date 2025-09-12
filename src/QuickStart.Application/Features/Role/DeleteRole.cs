using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuickStart.Application.Common.Audit;
using QuickStart.Application.Common.Handlers;
using QuickStart.Application.Data.Extensions;
using QuickStart.Application.Features.Right;

namespace QuickStart.Application.Features.Role;

public class DeleteRole(IHandlerContext context,  IAuditContext auditContext ) 
: CommandHandler<int>(context, auditContext)
{
    private IEnumerable<RoleRight> roleRights = [];
    public override async Task<OperationResult> HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var existingRole = await context.DBWrite.Roles.SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (existingRole == null) return ErrorResult.Create(ErrorResultCode.NotFound);

        var usersAssociatedWithRole = await GetUsersAssociatedWithRole(id, cancellationToken);
        var roleHasUsers = usersAssociatedWithRole > 0;

        if (roleHasUsers)
        {
            var errorMessage = $@"Role can not be deleted. {usersAssociatedWithRole} 
            Users are part of this role";
            return ErrorResult.Create(errorMessage);
        }
        using var transaction = await context.DBWrite.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            existingRole.Status = false;
            context.DBWrite.Roles.Update(existingRole);
            await SoftDeleteRoleRigtsAsync(id, cancellationToken);
            await context.DBWrite.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return SuccessResult.Create(id);
        }
        catch (SqlException)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }


    }
    private async Task SoftDeleteRoleRigtsAsync(int id, CancellationToken cancellationToken)
    {
        roleRights = await context.DBWrite.RoleRights
          .Where(rr => rr.RoleId == id)
          .ToListAsync(cancellationToken);

        foreach (var roleRight in roleRights)
        {
            roleRight.IsRemoved = true;
        }

        context.DBWrite.RoleRights.UpdateRange(roleRights);
        
    
}
       


   private async Task<int> GetUsersAssociatedWithRole(int id, CancellationToken cancellationToken)
    {
        return await context.DBRead.UserRoles.CountAsync(ur => ur.RoleId == id, cancellationToken);
    } 
    protected override void SetAuditDetail(int id)
    {
        var roleTable = context.DBRead.GetTableName<Role>();
        var roleRigtsTable = context.DBRead.GetTableName<RoleRight>();

        var roleRightIds = roleRights.Aggregate(string.Empty, (runningValue, rr) => $"{rr.RoleRightId},{runningValue}");

        var affectedEntities = AuditAffectedEntity.Instance
        .Add(roleTable, id.ToString())
        .Add(roleRigtsTable, roleRightIds )
        .ToString();

        auditContext.EntityDetail = new EntityAuditDetail
        {
            Action = nameof(AddOrUpdateRole),
            NewValues = id.ToString(),
            OldValues = id.ToString(),
            AffectedEntities = affectedEntities
        };
    }

 
}

