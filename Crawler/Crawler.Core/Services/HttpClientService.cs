using System.Xml.Serialization;
using Crawler.Core.Caching;
using Crawler.Core.Models;
using Crawler.Core.Settings;
using ExchangeTypes.Models;
using Microsoft.Extensions.Options;

namespace Crawler.Core.Services;

public class HttpClientService : IHttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCacheManager _cacheManager;
    private readonly ExternalUrlsSettings _externalUrlsSettings;

    public HttpClientService(
        IHttpClientFactory httpClientFactory,
        IMemoryCacheManager cacheManager,
        IOptions<ExternalUrlsSettings> settings)
    {
        _httpClientFactory = httpClientFactory;
        _cacheManager = cacheManager;
        _externalUrlsSettings = settings.Value;
    }

    public async Task<IReadOnlyCollection<DailyCurrencyModel>> GetDailyCurrencyData(DateTime date)
    {
        var dict = await GetCommonCurrencyData();
        var client = _httpClientFactory.CreateClient();
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            _externalUrlsSettings.DailyDataUrl + date.ToString("dd/MM/yyyy"));
        HttpResponseMessage response = await client.SendAsync(requestMessage);

        var xmls = new XmlSerializer(typeof(DailyCurrencyListModel));
        var result = new List<DailyCurrencyModel>();
        await using var stream = await response.Content.ReadAsStreamAsync();
        
        if (xmls.Deserialize(stream) is DailyCurrencyListModel deserialized)
        {
            foreach (var item in deserialized.Items)
            {
                if (dict.TryGetValue(item.CharCode, out var value))
                {
                    result.Add(new DailyCurrencyModel()
                    {
                        Name = item.Name,
                        IsoCharCode = item.CharCode,
                        Nominal = item.Nominal,
                        EngName = value.EngName,
                        Date = deserialized.Date,
                        Value = item.Value
                    });
                }
            }
        }

        return result;
    }

    public async Task<Dictionary<string, CommonCurrencyModel>> GetCommonCurrencyData()
    {
        if (!_cacheManager.TryGetValue(CrawlerConstants.CommonDataCacheKey,
                out Dictionary<string, CommonCurrencyModel> commonData))
        {
            var client = _httpClientFactory.CreateClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, _externalUrlsSettings.CommonDataUrl);
            HttpResponseMessage response = await client.SendAsync(requestMessage);

            var xmls = new XmlSerializer(typeof(BaseCurrencyModel));

            await using var stream = await response.Content.ReadAsStreamAsync();
            if (xmls.Deserialize(stream) is BaseCurrencyModel deserialized)
            {
                commonData = deserialized.Items.Where(e => !string.IsNullOrEmpty(e.IsoCharCode))
                    .DistinctBy(e => e.IsoCharCode)
                    .Select(e => new CommonCurrencyModel()
                {
                    Name = e.Name,
                    EngName = e.EngName,
                    IsoCharCode = e.IsoCharCode,
                    ParentCode = e.ParentCode.Trim()
                }).ToDictionary(e => e.IsoCharCode);
                
            }

            commonData.Add("RUB", new CommonCurrencyModel()
            {
                IsoCharCode = "RUB",
                EngName = "Russian Ruble",
                Name = "Российский рубль",
                ParentCode = "syntetic"
            });

            _cacheManager.AddEntry(CrawlerConstants.CommonDataCacheKey, commonData);
        }

        return commonData;
    }
}
