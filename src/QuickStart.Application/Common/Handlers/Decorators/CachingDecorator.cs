using Microsoft.Extensions.Logging;
using QuickStart.Application.Common.Cache;

namespace QuickStart.Application.Common.Handlers.Decorators;

public class CachingDecorator<TRequest,TResponse>(
    IHandler<TRequest> innerHandler,
    string cacheKey,
    ICache cache,
    AppCacheOptions options, ILogger logger) : HandlerDecorator<TRequest>(innerHandler)
  
{
    private readonly ICache cache = cache;

   
    public async override Task<OperationResult> HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var requestName = request.GetType().Name;

        if (request is not IAppQuery)
        {
            logger.LogInformation("Skipping caching for non query request {requestName}", requestName);
            return await innerHandler.HandleAsync(request, cancellationToken);
        }
        if (await cache.ExistsAsync(cacheKey, cancellationToken))
            {
            logger.LogInformation("Cache hit for request {requestName}", requestName);

                var cachedResult = await cache.GetAsync<TResponse>(cacheKey, cancellationToken);
                return new SuccessResult<TResponse>(cachedResult);
            }
            logger.LogInformation("Cache miss for request {requestName}", requestName);

        var result = await innerHandler.HandleAsync(request, cancellationToken);
        await cache.SetAsync(cacheKey, result, options, cancellationToken);

        return result;
    }

}
