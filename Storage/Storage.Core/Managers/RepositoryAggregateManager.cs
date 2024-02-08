using ExchangeTypes.Models;
using Storage.Core.Mapping;
using Storage.Database.Entities;
using Storage.Database.Repository;
using Storage.Database.Specifications;

namespace Storage.Core.Managers;

public class RepositoryAggregateManager : IRepositoryAggregateManager
{
    private readonly IBaseDataRepository _baseDataRepository;
    private readonly IDailyDataRepository _dailyDataRepository;

    public RepositoryAggregateManager(IBaseDataRepository baseDataRepository, IDailyDataRepository dailyDataRepository)
    {
        _baseDataRepository = baseDataRepository;
        _dailyDataRepository = dailyDataRepository;
    }

    public async Task ConvertAndSaveDailyData(SaveDailyDataRequest request)
    {
        var existing = await _dailyDataRepository.GetFilteredItems(new DateSpecifications(request.Date, request.Date));
        if (existing.Any()) return;

        var result = new List<DailyCurrencyEntity>();
        var allBaseCurrencies = await _baseDataRepository.GetAllAsync();
        foreach (var baseCurrency in allBaseCurrencies)
        {
            var currentCurrencyRelations =
                request.CurrencyRelations.Where(e => e.CurrencyCode == baseCurrency.ISOCharCode);

            foreach (var relation in currentCurrencyRelations)
            {
                var targetCurrency = allBaseCurrencies.First(e => e.ISOCharCode == relation.TargetCurrencyCode);
                result.Add(relation.ToEntity(baseCurrency, targetCurrency));
            }
        }

        await _dailyDataRepository.CreateBulk(result);
    }

    public async Task DeleteLatestItems(DateTime date)
    {
        var utcDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        await _dailyDataRepository.DeleteLatestItems(utcDate);
    }
}