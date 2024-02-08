namespace ExchangeTypes.Models;

public interface FinalizeSavingRequest
{
    public Guid CorrelationId { get; set; }
}