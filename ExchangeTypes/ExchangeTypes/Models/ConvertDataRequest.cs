namespace ExchangeTypes.Models;

public class ConvertDataRequest
{
    public Guid CorrelationId { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<DailyCurrencyModel> DailyCurrencyModels { get; set; }
}
