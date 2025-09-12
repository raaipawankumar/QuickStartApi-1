using QuickStart.Application.Common.Audit;

namespace QuickStart.Application.Common.Handlers;

public abstract class CommandHandler<TRequest>(IHandlerContext context, IAuditContext auditContext) 
: IHandler<TRequest>
{
    protected IHandlerContext context = context;
    protected IAuditContext auditContext = auditContext;
    public abstract Task<OperationResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
    protected abstract void SetAuditDetail(TRequest request);
  

}



