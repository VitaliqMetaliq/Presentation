namespace Storage.Core.Caching.Models
{
    public class DailyCurrencyCacheModel
    {
        public int Id { get; set; }
        public int BaseCurrencyId { get; set; }
        public int CurrencyId { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }

        public BaseCurrencyCacheModel BaseCurrency { get; set; }
        public BaseCurrencyCacheModel TargetCurrency { get; set; }
    }
}
