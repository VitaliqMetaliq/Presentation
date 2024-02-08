using FluentValidation;
using Storage.Main.Models;

namespace Storage.Main.Validation;

public class DateRangeValidator : AbstractValidator<DateRangeModel>
{
    public DateRangeValidator()
    {
        RuleFor(e => e)
            .Must(e => ValidationHelper.IsDatesInValidRange(e.DateFrom, e.DateTo))
            .WithMessage("DateFrom must not be greater than DateTo");
    }
}