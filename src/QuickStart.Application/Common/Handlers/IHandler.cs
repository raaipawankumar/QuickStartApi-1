namespace QuickStart.Application.Common.Handlers;

public interface IHandler<TRequest>
{
    Task<OperationResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

// public abstract class HandlerBase<TRequest>(IHandlerContext context) : IHandler<TRequest>
// {
//     protected IHandlerContext context = context;
//     public abstract Task<OperationResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);

// }




