namespace Crawler.Core.Services;

public interface ICrawlerJobManager
{
    Task EnqueueFireAndForgetJob();
    Task EnqueueDailyJob();
}

