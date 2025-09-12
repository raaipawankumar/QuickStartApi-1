using QuickStart.Application.Common.Audit;
using QuickStart.Application.Common.Handlers;

namespace QuickStart.Application.Features.Language;

public class DeleteLanguage(IHandlerContext context, IAuditContext auditContext) 
: CommandHandler<Guid>(context, auditContext)
{
    public override async Task<OperationResult> HandleAsync(Guid id,
        CancellationToken cancellationToken = default)
    {

    //     var entityToDelete = (from language in context.Db.Languages
    //                          where language.Id == id
    //                          select language).SingleOrDefault();

    //     if (entityToDelete == null) return new ErrorResult(ErrorResultCode.NotFound);

    //    // entityToDelete.IsDeleted = true;
    //     context.Db.Languages.Update(entityToDelete);
    //     await context.Db.SaveChangesAsync(cancellationToken);
        return SuccessResult.Instance;
    }

    protected override void SetAuditDetail(Guid request)
    {
        throw new NotImplementedException();
    }
}
