using Crawler.Database.Entities;
using Crawler.Database.Repository;
using ExchangeTypes.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Crawler.Core.Services;

public class CrawlerJobManager : ICrawlerJobManager
{
    private readonly IHttpClientService _httpClientService;
    private readonly ILogger<CrawlerJobManager> _logger;
    private readonly IBus _bus;
    private readonly ISavePointRepository _savePointRepository;

    public CrawlerJobManager(
        IHttpClientService httpClientService,
        ILogger<CrawlerJobManager> logger,
        IBus bus,
        ISavePointRepository savePointRepository)
    {
        _httpClientService = httpClientService;
        _logger = logger;
        _bus = bus;
        _savePointRepository = savePointRepository;
    }

    public async Task EnqueueFireAndForgetJob()
    {
        try
        {
            var commonData = await _httpClientService.GetCommonCurrencyData();
            var converterEndpoint = await _bus.GetSendEndpoint(new Uri("queue:BaseDataStorage"));
            await converterEndpoint.Send(new SaveBaseDataRequest()
            {
                BaseCurrencies = commonData.Values
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Something went wrong with fire-and-forget job: {ex}", ex.Message);
        }
    }

    public async Task EnqueueDailyJob()
    {
        try
        {
            var dates = new List<DateTime>();
            SavePointEntity lastSavePoint = await _savePointRepository.GetLastOrCreateAsync();
            if (lastSavePoint.Timestamp.Date != DateTime.Now.AddDays(-1).Date)
            {
                for (DateTime date = lastSavePoint.Timestamp.AddDays(1); date <= DateTime.Now; date = date.AddDays(1))
                {
                    dates.Add(date);
                }
            }
            else
            {
                dates.Add(lastSavePoint.Timestamp.AddDays(1));
            }

            var endpoint = await _bus.GetSendEndpoint(new Uri("queue:StorageState"));
            await endpoint.Send<SaveBulkDataRequest>(new { CorrelationId = Guid.NewGuid(), Dates = dates });
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Something went wrong with recurring job: {ex}", ex.Message);
        }
    }
}
