using Storage.Database.Repository;
using Storage.Database.Specifications;
using Storage.Main.Mapping;
using Storage.Main.Models;

namespace Storage.Main.Managers;

public class StorageManager : IStorageManager
{
    private readonly IDailyDataRepository _dailyDataRepository;

    public StorageManager(IDailyDataRepository dailyDataRepository) => _dailyDataRepository = dailyDataRepository;

    public async Task<IReadOnlyCollection<DailyCurrencyViewModel>> GetFilteredCurrenciesByIsoCode(string isoCode)
    {
        var result =
            await _dailyDataRepository.GetFilteredItems(new IsoCodeSpecifications(isoCode));

        return result.Select(e => e.ToModel()).ToList();
    }

    public async Task<IReadOnlyCollection<DailyCurrencyViewModel>> GetFilteredCurrenciesByDate(DateTime dateFrom,
        DateTime dateTo)
    {
        var result = await _dailyDataRepository.GetFilteredItems(new DateSpecifications(dateFrom, dateTo));

        return result.Select(e => e.ToModel()).ToList();
    }
}
