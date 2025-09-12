namespace QuickStart.Application.Common.Handlers;

public abstract class HandlerDecorator<TRequest>(IHandler<TRequest> innerHandler) : IHandler<TRequest>
{
   protected readonly IHandler<TRequest> innerHandler = innerHandler;

    public abstract Task<OperationResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
  
}
