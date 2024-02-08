using FluentValidation;
using FluentValidation.Validators;
using Storage.Database.Repository;

namespace Storage.Main.Validation;

internal class IsoCodePropertyValidator<T> : AsyncPropertyValidator<T, string>
{
    private readonly IBaseDataRepository _baseDataRepository;

    public IsoCodePropertyValidator(IBaseDataRepository baseDataRepository)
    {
        _baseDataRepository = baseDataRepository;
    }

    public override async Task<bool> IsValidAsync(ValidationContext<T> context, string value, CancellationToken cancellation)
    {
        var existing = await _baseDataRepository.FindByCurrencyCodeAsync(value);
        if (existing != null)
        {
            return true;
        }
        context.MessageFormatter.AppendArgument("IsoCode", value);
        return false;
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "IsoCode: '{IsoCode}' is not found or invalid";
    }

    public override string Name => "IsoCodePropertyValidator";
}