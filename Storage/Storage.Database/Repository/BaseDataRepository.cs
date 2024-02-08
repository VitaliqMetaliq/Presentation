using Microsoft.EntityFrameworkCore;
using Storage.Database.Entities;

namespace Storage.Database.Repository;

public class BaseDataRepository : IBaseDataRepository
{
    private readonly StorageDbContext _dbContext;

    public BaseDataRepository(StorageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<BaseCurrencyEntity>> GetAllAsync()
    {
        return await _dbContext.BaseCurrencies.AsNoTracking().ToArrayAsync();
    }

    public async Task UpdateAsync(BaseCurrencyEntity item)
    {
        var existingItem = await _dbContext.BaseCurrencies.FirstOrDefaultAsync(x => x.Id == item.Id);
        if (existingItem != null)
        {
            _dbContext.BaseCurrencies.Update(item);
        }
    }

    public async Task UpdateBulkAsync(IEnumerable<BaseCurrencyEntity> items)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            _dbContext.BaseCurrencies.UpdateRange(items);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }

    public async Task AddOrUpdateAsync(BaseCurrencyEntity item)
    {
        var existing = await _dbContext.BaseCurrencies.Where(e => e.ISOCharCode == item.ISOCharCode)
            .FirstOrDefaultAsync();
        if (existing != null)
        {
            existing.EngName = item.EngName;
            existing.Name = item.Name;
            existing.ParentCode = item.ParentCode;
        }
        else
        {
            await _dbContext.BaseCurrencies.AddAsync(item);
        }
    }

    public async Task<BaseCurrencyEntity> FindByIdAsync(int id)
    {
        return await _dbContext.BaseCurrencies.FirstOrDefaultAsync(x => x.Id == id) ??
               throw new Exception($"Item with Id = {id} not found.");
    }

    public async Task<BaseCurrencyEntity?> FindByCurrencyCodeAsync(string currencyCode)
    {
        return await _dbContext.BaseCurrencies.AsNoTracking().FirstOrDefaultAsync(x => x.ISOCharCode == currencyCode);
    }

    public async Task CreateAsync(BaseCurrencyEntity item)
    {
        await _dbContext.AddAsync(item);
    }

    public async Task CreateBulkAsync(IEnumerable<BaseCurrencyEntity> items)
    {
        await _dbContext.AddRangeAsync(items);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
