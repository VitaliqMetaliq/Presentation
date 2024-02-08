using Microsoft.Extensions.Caching.Memory;
using ExchangeTypes.Models;

namespace Crawler.Core.Caching;

public class MemoryCacheManager : IMemoryCacheManager
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheManager(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Dictionary<string, CommonCurrencyModel> AddEntry(string key, Dictionary<string, CommonCurrencyModel> dictionary)
    {
        return _memoryCache.Set(key, dictionary);
    }

    public bool TryGetValue(string key, out Dictionary<string, CommonCurrencyModel> value)
    {
        return _memoryCache.TryGetValue(key, out value);
    }
}
