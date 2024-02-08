using FluentValidation;
using Storage.Database.Repository;
using Storage.Main.Models;

namespace Storage.Main.Validation;

public class IsoCodeValidator : AbstractValidator<IsoCodeWrapperModel>
{
    public IsoCodeValidator(IBaseDataRepository baseDataRepository)
    {
        RuleFor(e => e.IsoCode).SetAsyncValidator(new IsoCodePropertyValidator<IsoCodeWrapperModel>(baseDataRepository));
    }
}