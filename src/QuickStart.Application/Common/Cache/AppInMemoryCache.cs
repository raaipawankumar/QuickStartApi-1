using Microsoft.Extensions.Caching.Memory;

namespace QuickStart.Application.Common.Cache;

public class AppInMemoryCache(IMemoryCache memoryCache) : ICache
{
    private readonly IMemoryCache memoryCache = memoryCache ??
        throw new ArgumentNullException($"{nameof(memoryCache)} is null");

    public Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (memoryCache.TryGetValue(key, out object? value) && value is T result)
        {
            return Task.FromResult(result);
       }
        return Task.FromResult(default(T));
    }
    public async Task<T> GetOrSetAsync<T>(string key,
     Func<Task<T>> setValueCallback,
     AppCacheOptions? options = null,
      CancellationToken cancellationToken = default)
    {
        if (memoryCache.TryGetValue(key, out T cachedValue))
        {
            return cachedValue;
        }

        var value = await setValueCallback();
        await SetAsync<object>(key, value, options, cancellationToken);
        return value;
    }
    public Task SetAsync<TValue>(string cacheKey, TValue value,
     AppCacheOptions? options = null, CancellationToken cancellationToken = default)
    {
        var entryOptions = new MemoryCacheEntryOptions();

        if (options != null)
        {
            if (options.AbsoluteExpiration.HasValue)
                entryOptions.AbsoluteExpirationRelativeToNow = options.AbsoluteExpiration;

            if (options.SlidingExpiration.HasValue)
                entryOptions.SlidingExpiration = options.SlidingExpiration;

            entryOptions.Priority = ConvertPriority(options.Priority);

            if (options.Size.HasValue)
                entryOptions.Size = options.Size;
        }

        memoryCache.Set(cacheKey, value, entryOptions);
        return Task.CompletedTask;


    }
    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        memoryCache.Remove(key);
        return Task.CompletedTask;
    }
    public Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task ClearAsync(CancellationToken cancellationToken = default)
    {
        if (memoryCache is MemoryCache mc)
        {
            mc.Clear();
        }
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        var exists = memoryCache.TryGetValue(key, out _);
        return Task.FromResult(exists);
    }
    private static Microsoft.Extensions.Caching.Memory.CacheItemPriority ConvertPriority(CacheItemPriority priority)
    {
        return priority switch
        {
            CacheItemPriority.Low => Microsoft.Extensions.Caching.Memory.CacheItemPriority.Low,
            CacheItemPriority.High => Microsoft.Extensions.Caching.Memory.CacheItemPriority.High,
            CacheItemPriority.NeverRemove => Microsoft.Extensions.Caching.Memory.CacheItemPriority.NeverRemove,
            _ => Microsoft.Extensions.Caching.Memory.CacheItemPriority.Normal
        };
    }

}

