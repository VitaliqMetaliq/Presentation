using ExchangeTypes.Models;

namespace Crawler.Core.Services;

public interface IHttpClientService
{
    Task<IReadOnlyCollection<DailyCurrencyModel>> GetDailyCurrencyData(DateTime date);
    Task<Dictionary<string, CommonCurrencyModel>> GetCommonCurrencyData();
}
