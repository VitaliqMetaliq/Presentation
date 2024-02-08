namespace Storage.Database.Entities;

public class BaseCurrencyEntity : IIdentity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string EngName { get; set; }

    public string ParentCode { get; set; }

    public string ISOCharCode { get; set; }

    public ICollection<DailyCurrencyEntity> BaseDailyCurrencyEntities { get; set; }

    public ICollection<DailyCurrencyEntity> DailyCurrencyEntities { get; set; }
}
