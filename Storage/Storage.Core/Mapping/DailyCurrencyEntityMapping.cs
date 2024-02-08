using ExchangeTypes.Models;
using Storage.Core.Caching.Models;
using Storage.Database.Entities;

namespace Storage.Core.Mapping;

public static class DailyCurrencyEntityMapping
{
    public static DailyCurrencyEntity ToEntity(
        this CurrencyRelationModel model, 
        BaseCurrencyEntity baseEntity,
        BaseCurrencyEntity targetEntity)
    {
        return new DailyCurrencyEntity()
        {
            Date = DateTime.SpecifyKind(model.Date, DateTimeKind.Utc),
            BaseCurrencyId = baseEntity.Id,
            CurrencyId = targetEntity.Id,
            Value = model.Value
        };
    }

    public static DailyCurrencyEntity ToEntity(this DailyCurrencyCacheModel model)
    {
        return new DailyCurrencyEntity()
        {
            Id = model.Id,
            Date = model.Date,
            BaseCurrencyId = model.BaseCurrencyId,
            CurrencyId = model.CurrencyId,
            Value = model.Value,
            BaseCurrency = model.BaseCurrency.ToEntity(),
            Currency = model.TargetCurrency.ToEntity()
        };
    }

    public static DailyCurrencyCacheModel ToModel(this DailyCurrencyEntity entity)
    {
        return new DailyCurrencyCacheModel()
        {
            Id = entity.Id,
            Date = entity.Date,
            CurrencyId = entity.CurrencyId,
            BaseCurrencyId = entity.BaseCurrencyId,
            Value = entity.Value,
            BaseCurrency = entity.BaseCurrency.ToModel(),
            TargetCurrency = entity.Currency.ToModel()
        };
    }
}
