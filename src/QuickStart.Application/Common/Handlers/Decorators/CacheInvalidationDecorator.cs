using Microsoft.Extensions.Logging;
using QuickStart.Application.Common.Cache;

namespace QuickStart.Application.Common.Handlers.Decorators;

public class CacheInvalidationDecorator<TRequest>(
    IHandler<TRequest> innerHandler,
     Func<string[]> getCacheKeys,
    ICache cache,
    ILogger logger)
     : HandlerDecorator<TRequest>(innerHandler)
{

    public async override Task<OperationResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var requsetName = request.GetType().Name;
        var result = await innerHandler.HandleAsync(request, cancellationToken);
        var eligibleForCacheInvalidation = request is IAppCommand && result.IsSuccess;
        if (!eligibleForCacheInvalidation)
        {
            logger.LogInformation("Skipping cache invalidation for non command request {requsetName}", requsetName);
            return result;
        }
        foreach (var cacheKey in getCacheKeys())
        {
            await cache.RemoveAsync(cacheKey, cancellationToken);
            logger.LogInformation("Cleared cache key {cacheKey}", cacheKey);

        }

        return result;
    }
}

