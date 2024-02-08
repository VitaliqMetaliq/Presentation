namespace Storage.Database.Entities;

public class DailyCurrencyEntity : IIdentity
{
    public int Id { get; set; }
    public int BaseCurrencyId { get; set; }
    public int CurrencyId { get; set; }
    public DateTime Date { get; set; }
    public double Value { get; set; }

    public virtual BaseCurrencyEntity BaseCurrency { get; set; }
    public virtual BaseCurrencyEntity Currency { get; set; }
}
