namespace QuickStart.Application.Common.Handlers;

public abstract class QueryHandler<TRequest>(IHandlerContext context) : IHandler<TRequest>
{
    protected IHandlerContext context = context;
    public abstract Task<OperationResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);

}



