using ExchangeTypes.Models;

namespace Crawler.Core.Caching;

public interface IMemoryCacheManager
{
    bool TryGetValue(string key, out Dictionary<string, CommonCurrencyModel> value);
    Dictionary<string, CommonCurrencyModel> AddEntry(string key, Dictionary<string, CommonCurrencyModel> dictionary);
}
