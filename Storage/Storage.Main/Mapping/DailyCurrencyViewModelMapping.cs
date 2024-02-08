using Storage.Database.Entities;
using Storage.Main.Models;

namespace Storage.Main.Mapping;

public static class DailyCurrencyViewModelMapping
{
    public static DailyCurrencyViewModel ToModel(this DailyCurrencyEntity entity)
    {
        return new DailyCurrencyViewModel()
        {
            Id = entity.Id,
            CurrencyCode = entity.BaseCurrency.ISOCharCode,
            TargetCurrencyCode = entity.Currency.ISOCharCode,
            Date = entity.Date,
            Value = entity.Value
        };
    }
}
