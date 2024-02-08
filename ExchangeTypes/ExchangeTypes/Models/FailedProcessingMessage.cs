namespace ExchangeTypes.Models;

public interface FailedProcessingMessage
{
    public Guid CorrelationId { get; set; }
}