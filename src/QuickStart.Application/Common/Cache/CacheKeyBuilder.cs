using System.Text.Json;

namespace QuickStart.Application.Common.Cache;

public static class CacheKeyBuilder
{
    private const string KEY_SEPARATOR = ":";
    private const string PARAM_SEPARATOR = "_";

    public static string Build(params string[] segments)
    {
        return string.Join(KEY_SEPARATOR, segments.Where(s => !string.IsNullOrEmpty(s)));
    }

    public static string Build(string baseKey, params object[] parameters)
    {
        if (parameters?.Length > 0)
        {
            var paramString = string.Join(PARAM_SEPARATOR, parameters.Select(p => p?.ToString() ?? "null"));
            return $"{baseKey}{KEY_SEPARATOR}{paramString}";
        }
        return baseKey;
    }
    public static string Build(string baseKey, object instance)
    {
        ArgumentNullException.ThrowIfNull(instance);
        return $"{baseKey}{KEY_SEPARATOR}{JsonSerializer.Serialize(instance)}";
    }
}

