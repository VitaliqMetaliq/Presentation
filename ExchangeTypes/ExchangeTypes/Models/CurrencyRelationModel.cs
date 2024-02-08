namespace ExchangeTypes.Models;

public class CurrencyRelationModel
{
    public string CurrencyCode { get; set; }
    public string TargetCurrencyCode { get; set; }
    public double Value { get; set; }
    public DateTime Date { get; set; }
}
