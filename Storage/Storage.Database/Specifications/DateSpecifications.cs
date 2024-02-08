using Storage.Database.Entities;
using Storage.Database.Specifications.Base;

namespace Storage.Database.Specifications;

public class DateSpecifications : BaseSpecifications<DailyCurrencyEntity>
{
    private DateTime DateFrom { get; }
    private DateTime DateTo { get; }

    public DateSpecifications(DateTime dateFrom, DateTime dateTo) : base()
    {
        DateFrom = DateTime.SpecifyKind(dateFrom, DateTimeKind.Utc);
        DateTo = DateTime.SpecifyKind(dateTo, DateTimeKind.Utc);

        AddInclude(e => e.BaseCurrency);
        AddInclude(e => e.Currency);
        SetFilterCondition(e => e.Date >= DateFrom && e.Date <= DateTo);
    }
}

