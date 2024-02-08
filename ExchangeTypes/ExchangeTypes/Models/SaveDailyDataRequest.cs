namespace ExchangeTypes.Models;

public interface SaveDailyDataRequest
{
    public Guid CorrelationId { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<CurrencyRelationModel> CurrencyRelations { get; set; }
}