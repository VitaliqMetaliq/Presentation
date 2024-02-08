using ExchangeTypes.Models;

namespace Converter.Core.Services;

public class ConverterService : IConverterService
{
    public IReadOnlyCollection<CurrencyRelationModel> ConvertCurrencyRelations(IEnumerable<DailyCurrencyModel> dailyModels)
    {
        var rubleRelations = dailyModels.Select(e => new CurrencyRelationModel()
        {
            CurrencyCode = "RUB",
            TargetCurrencyCode = e.IsoCharCode,
            Date = e.Date,
            Value = e.Nominal/e.Value
        });

        return dailyModels.SelectMany(daily => 
            dailyModels.Where(innerDaily => daily.IsoCharCode != innerDaily.IsoCharCode)
                .Select(innerDaily => new CurrencyRelationModel()
                {
                    CurrencyCode = daily.IsoCharCode,
                    TargetCurrencyCode = innerDaily.IsoCharCode,
                    Value = CalculateRelationValue(daily, innerDaily),
                    Date = daily.Date
                })).Union(rubleRelations).ToList();
    }

    private static double CalculateRelationValue(DailyCurrencyModel baseModel,
        DailyCurrencyModel targetModel)
    {
        return (targetModel.Nominal * baseModel.Value) / (baseModel.Nominal * targetModel.Value);
    }
}
