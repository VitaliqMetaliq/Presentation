using Crawler.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Database.Repository;

public class SavePointRepository : ISavePointRepository
{
    private readonly HangfireDbContext _dbContext;

    public SavePointRepository(HangfireDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SavePointEntity> GetLastOrCreateAsync()
    {
        var lastSavePoint = await _dbContext.SavePoints.FirstOrDefaultAsync();
        if (lastSavePoint == null)
        {
            lastSavePoint = new SavePointEntity() { Timestamp = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc) };
            await _dbContext.SavePoints.AddAsync(lastSavePoint);
            await _dbContext.SaveChangesAsync();
        }
        return lastSavePoint;
    }

    public async Task UpdateAsync()
    {
        var lastSavePoint = await _dbContext.SavePoints.FirstOrDefaultAsync();
        if (lastSavePoint != null)
        {
            lastSavePoint.Timestamp = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(DateTime date)
    {
        var lastSavePoint = await _dbContext.SavePoints.FirstOrDefaultAsync();
        if (lastSavePoint != null)
        {
            lastSavePoint.Timestamp = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            await _dbContext.SaveChangesAsync();
        }
    }
}