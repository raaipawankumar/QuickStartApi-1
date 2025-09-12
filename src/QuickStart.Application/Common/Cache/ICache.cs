namespace QuickStart.Application.Common.Cache;

public interface ICache
{
    Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task SetAsync<T>(string key, T value, AppCacheOptions? options = null, CancellationToken cancellationToken = default);
    Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, AppCacheOptions? options = null, CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default);
    Task ClearAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
}

