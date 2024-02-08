using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace Storage.Core.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public T Get<T>(string key)
    {
        var value = _cache.GetString(key);
        if (value != null)
        {
            return JsonSerializer.Deserialize<T>(value);
        }

        return default;
    }

    public T Set<T>(string key, T value)
    {
        _cache.SetString(key, JsonSerializer.Serialize(value));

        return value;
    }
}