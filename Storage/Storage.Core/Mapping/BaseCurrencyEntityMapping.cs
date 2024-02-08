using ExchangeTypes.Models;
using Storage.Core.Caching.Models;
using Storage.Database.Entities;

namespace Storage.Core.Mapping;

public static class BaseCurrencyEntityMapping
{
    public static BaseCurrencyEntity ToEntity(this CommonCurrencyModel model)
    {
        return new BaseCurrencyEntity()
        {
            Name = model.Name,
            EngName = model.EngName,
            ParentCode = model.ParentCode,
            ISOCharCode = model.IsoCharCode
        };
    }

    public static BaseCurrencyEntity ToEntity(this CommonCurrencyModel model, int id)
    {
        return new BaseCurrencyEntity()
        {
            Id = id,
            Name = model.Name,
            EngName = model.EngName,
            ParentCode = model.ParentCode,
            ISOCharCode = model.IsoCharCode
        };
    }

    public static BaseCurrencyCacheModel ToModel(this BaseCurrencyEntity entity)
    {
        return new BaseCurrencyCacheModel()
        {
            Id = entity.Id,
            Name = entity.Name,
            EngName = entity.EngName,
            ISOCharCode = entity.ISOCharCode,
            ParentCode = entity.ParentCode
        };
    }

    public static BaseCurrencyEntity ToEntity(this BaseCurrencyCacheModel model)
    {
        return new BaseCurrencyEntity()
        {
            Id = model.Id,
            Name = model.Name,
            EngName = model.EngName,
            ParentCode = model.ParentCode,
            ISOCharCode = model.ISOCharCode
        };
    }
}

