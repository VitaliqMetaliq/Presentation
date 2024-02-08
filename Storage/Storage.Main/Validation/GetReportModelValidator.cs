using FluentValidation;
using Storage.Database.Repository;
using Storage.Main.Models;

namespace Storage.Main.Validation;

public class GetReportModelValidator : AbstractValidator<GetReportModel>
{
    public GetReportModelValidator(IBaseDataRepository baseDataRepository)
    {
        CascadeMode = CascadeMode.Stop;
        RuleFor(e => e)
            .Must(e => ValidationHelper.IsDatesInValidRange(e.DateFrom, e.DateTo))
            .WithMessage("DateFrom must not be grater than DateTo");
        RuleFor(e => e).Must(PassDateRange).WithMessage("Report can be made for 30 days max");
        RuleFor(e => e.IsoCode).SetAsyncValidator(new IsoCodePropertyValidator<GetReportModel>(baseDataRepository));
    }

    private static bool PassDateRange(GetReportModel model)
    {
        return (model.DateTo - model.DateFrom).Days <= 30;
    }
}