using Storage.Database.Entities;
using Storage.Database.Specifications.Base;

namespace Storage.Database.Repository;

public interface IDailyDataRepository
{
    Task<DailyCurrencyEntity> FindById(int id);
    Task<IEnumerable<DailyCurrencyEntity>> GetFilteredItems(IBaseSpecifications<DailyCurrencyEntity> baseSpecifications);
    Task CreateBulk(IEnumerable<DailyCurrencyEntity> items);
    Task DeleteLatestItems(DateTime date);
}
