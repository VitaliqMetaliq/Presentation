namespace ExchangeTypes.Models;

public class DailyCurrencyModel
{
    public string Name { get; set; }
    public string EngName { get; set; }
    public string IsoCharCode { get; set; }
    public int Nominal { get; set; }
    public DateTime Date { get; set; }
    public double Value { get; set; }
}
