using Storage.Database.Entities;

namespace Storage.Database.Repository;

public interface IBaseDataRepository
{
    Task<IEnumerable<BaseCurrencyEntity>> GetAllAsync();
    Task CreateBulkAsync(IEnumerable<BaseCurrencyEntity> items);
    Task AddOrUpdateAsync(BaseCurrencyEntity item);
    Task CreateAsync(BaseCurrencyEntity entity);
    Task<BaseCurrencyEntity> FindByIdAsync(int id);
    Task<BaseCurrencyEntity?> FindByCurrencyCodeAsync(string currencyCode);
    Task UpdateAsync(BaseCurrencyEntity entity);
    Task UpdateBulkAsync(IEnumerable<BaseCurrencyEntity> items);
    Task SaveChangesAsync();
}
