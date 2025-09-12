using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace QuickStart.Application.Common.Handlers.Decorators;

public class LoggingDecorator<TRequest>(
    IHandler<TRequest> innerHandler, ILogger logger)
 : HandlerDecorator<TRequest>(innerHandler)

{
    public override async Task<OperationResult> HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var requestName = request.GetType().Name;
        logger.LogInformation("Executing: {RequestName}", requestName);
        var stopwatch = Stopwatch.StartNew();
        stopwatch.Start();
        try
        {
            var result = await innerHandler.HandleAsync(request, cancellationToken);
            logger.LogInformation("{RequestName} executed successfully", requestName);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error executing: {RequestName}", requestName);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            logger.LogInformation("{RequestName} execution time (millisecond) : {time} ", requestName, stopwatch.ElapsedMilliseconds);
        }
    }

  
}
