using Storage.Database.Entities;
using Storage.Database.Specifications.Base;

namespace Storage.Database.Specifications;

public class IsoCodeSpecifications : BaseSpecifications<DailyCurrencyEntity>
{
    private string IsoCode { get; }

    public IsoCodeSpecifications(string code) : base()
    {
        IsoCode = code;
        AddInclude(c => c.BaseCurrency);
        AddInclude(c => c.Currency);
        SetFilterCondition(e => e.BaseCurrency.ISOCharCode == IsoCode);
    }
}
