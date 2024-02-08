namespace ExchangeTypes.Models;

public interface ProcessDailyDataRequest
{
    public Guid CorrelationId { get; set; }
    public DateTime Date { get; set; }
}