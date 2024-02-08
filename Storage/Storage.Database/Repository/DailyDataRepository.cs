using Microsoft.EntityFrameworkCore;
using Storage.Database.Entities;
using Storage.Database.Specifications.Base;

namespace Storage.Database.Repository;

public class DailyDataRepository : IDailyDataRepository
{
    private readonly StorageDbContext _dbContext;

    public DailyDataRepository(StorageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DailyCurrencyEntity> FindById(int id)
    {
        return await _dbContext.DailyCurrencies.FirstOrDefaultAsync(x => x.Id == id) ??
               throw new Exception($"Item with Id = {id} not found.");
    }

    public async Task<IEnumerable<DailyCurrencyEntity>> GetFilteredItems(
        IBaseSpecifications<DailyCurrencyEntity> baseSpecifications)
    {
        var items = await SpecificationEvaluator<DailyCurrencyEntity>.GetQuery(_dbContext.Set<DailyCurrencyEntity>()
                .AsQueryable(), baseSpecifications)
            .AsNoTracking()
            .ToListAsync();

        return items;
    }

    public async Task DeleteLatestItems(DateTime date)
    {
        var itemsToDelete = await _dbContext.DailyCurrencies.Where(e => e.Date > date).ToArrayAsync();
        _dbContext.DailyCurrencies.RemoveRange(itemsToDelete);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateBulk(IEnumerable<DailyCurrencyEntity> items)
    {
        await _dbContext.DailyCurrencies.AddRangeAsync(items);
        await _dbContext.SaveChangesAsync();
    }
}
