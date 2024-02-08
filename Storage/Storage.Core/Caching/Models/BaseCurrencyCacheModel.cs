namespace Storage.Core.Caching.Models
{
    public class BaseCurrencyCacheModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EngName { get; set; }
        public string ParentCode { get; set; }
        public string ISOCharCode { get; set; }
    }
}
