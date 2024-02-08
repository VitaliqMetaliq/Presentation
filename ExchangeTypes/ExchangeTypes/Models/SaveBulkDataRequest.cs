namespace ExchangeTypes.Models;

public interface SaveBulkDataRequest
{
    public Guid CorrelationId { get; set; }
    public List<DateTime> Dates { get; set; }
}