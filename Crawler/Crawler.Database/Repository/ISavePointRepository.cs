using Crawler.Database.Entities;

namespace Crawler.Database.Repository;

public interface ISavePointRepository
{
    Task<SavePointEntity> GetLastOrCreateAsync();
    Task UpdateAsync();
    Task UpdateAsync(DateTime date);
}