using ExchangeTypes.Models;

namespace Converter.Core.Services;

public interface IConverterService
{
    IReadOnlyCollection<CurrencyRelationModel> ConvertCurrencyRelations(IEnumerable<DailyCurrencyModel> dailyModels);
}
