using Crawler.Core.Services;
using ExchangeTypes.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.Main.Controllers;

[ApiController]
[Route("CrawlerApi")]
public class CrawlerController : ControllerBase
{
    private readonly IHttpClientService _httpClientService;
    private readonly ICrawlerJobManager _crawlerJobManager;

    public CrawlerController(IHttpClientService httpClientService, ICrawlerJobManager crawlerJobManager)
    {
        _httpClientService = httpClientService;
        _crawlerJobManager = crawlerJobManager;
    }

    [HttpGet("GetTodayData")]
    public async Task<IReadOnlyCollection<DailyCurrencyModel>> GetData()
    {
        return await _httpClientService.GetDailyCurrencyData(DateTime.Now);
    }

    [HttpGet("[action]")]
    public async Task ProcessDailyData()
    {
        await _crawlerJobManager.EnqueueDailyJob();
    }
}

