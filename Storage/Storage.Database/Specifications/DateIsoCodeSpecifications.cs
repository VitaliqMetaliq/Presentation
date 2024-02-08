using Storage.Database.Entities;
using Storage.Database.Specifications.Base;

namespace Storage.Database.Specifications;

public class DateIsoCodeSpecifications : BaseSpecifications<DailyCurrencyEntity>
{
    private DateTime DateFrom { get; }
    private DateTime DateTo { get; }
    private string IsoCode { get; }

    public DateIsoCodeSpecifications(DateTime dateFrom, DateTime dateTo, string isoCode)
    {
        DateFrom = dateFrom;
        DateTo = dateTo;
        IsoCode = isoCode;

        AddInclude(c => c.BaseCurrency);
        AddInclude(c => c.Currency);
        SetFilterCondition(e => 
            (e.Date >= DateFrom && e.Date <= DateTo) && (e.BaseCurrency.ISOCharCode == IsoCode));
    }
}