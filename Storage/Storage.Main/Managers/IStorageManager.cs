using Storage.Main.Models;

namespace Storage.Main.Managers;

public interface IStorageManager
{
    Task<IReadOnlyCollection<DailyCurrencyViewModel>> GetFilteredCurrenciesByIsoCode(string isoCode);

    Task<IReadOnlyCollection<DailyCurrencyViewModel>> GetFilteredCurrenciesByDate(DateTime dateFrom,
        DateTime dateTo);
}
