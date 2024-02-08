using ExchangeTypes.Models;

namespace Storage.Core.Managers;

public interface IRepositoryAggregateManager
{
    Task ConvertAndSaveDailyData(SaveDailyDataRequest request);
    Task DeleteLatestItems(DateTime date);
}