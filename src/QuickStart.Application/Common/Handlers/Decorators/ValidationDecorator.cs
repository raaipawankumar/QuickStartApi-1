using FluentValidation;
using Microsoft.Extensions.Logging;

namespace QuickStart.Application.Common.Handlers.Decorators;

public class ValidationDecorator<TRequest>(
    IHandler<TRequest> handler,
    AbstractValidator<TRequest> validator,
    ILogger logger) : HandlerDecorator<TRequest>(handler)
{
    private readonly AbstractValidator<TRequest> validator = validator;

    public async override Task<OperationResult> HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var requestName = request.GetType().Name;
        logger.LogInformation("Executing validation decorator for {requestName}", requestName);
         var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            logger.LogInformation("Validation failed for {requestName}", requestName);
            return new ErrorResult(errorMessages);
         }
            logger.LogInformation("Validation successful for {requestName}", requestName);

        return await innerHandler.HandleAsync(request, cancellationToken);
    }
}
