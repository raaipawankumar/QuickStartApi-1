namespace QuickStart.Application.Common.Cache;

public class AppCacheOptions
{
    public TimeSpan? AbsoluteExpiration { get; set; } = TimeSpan.FromMinutes(30);
    public TimeSpan? SlidingExpiration { get; set; } = TimeSpan.FromMinutes(30);
    public CacheItemPriority Priority { get; set; } = CacheItemPriority.Normal;
    public string[] Tags { get; set; } = [];
    public long? Size { get; set; } = 1024;
    public static AppCacheOptions Default => new();
    
}

