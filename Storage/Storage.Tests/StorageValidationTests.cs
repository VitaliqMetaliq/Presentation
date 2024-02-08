using System.Globalization;
using Storage.Database.Entities;
using Storage.Database.Repository;
using Storage.Main.Models;
using Storage.Main.Validation;

namespace Storage.Tests;

public class StorageValidationTests
{
    private readonly GetReportModelValidator _getReportModelValidator;
    private readonly DateRangeValidator _dateRangeValidator = new();
    private readonly IsoCodeValidator _isoCodeValidator;
    private readonly Mock<IBaseDataRepository> _baseDataRepositoryMock = new();

    public StorageValidationTests()
    {
        _getReportModelValidator = new(_baseDataRepositoryMock.Object);
        _isoCodeValidator = new(_baseDataRepositoryMock.Object);
    }

    [Fact]
    public async Task GetReportModelValidatorTest_PositiveCase()
    {
        _baseDataRepositoryMock.Setup(e => e.FindByCurrencyCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(new BaseCurrencyEntity());

        var validationResult = await _getReportModelValidator.ValidateAsync(new GetReportModel()
        {
            DateFrom = DateTime.Parse("01.13.2023", CultureInfo.InvariantCulture),
            DateTo = DateTime.Parse("01.20.2023", CultureInfo.InvariantCulture),
            IsoCode = "USD"
        });

        using (new AssertionScope())
        {
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }
    }

    [Fact]
    public async Task GetReportModelValidatorTest_WrongIsoCode_NegativeCase()
    {
        var isoCode = "USD";
        _baseDataRepositoryMock.Setup(e => e.FindByCurrencyCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(null as BaseCurrencyEntity);

        var validationResult = await _getReportModelValidator.ValidateAsync(new GetReportModel()
        {
            DateFrom = DateTime.Parse("01.13.2023", CultureInfo.InvariantCulture),
            DateTo = DateTime.Parse("01.20.2023", CultureInfo.InvariantCulture),
            IsoCode = isoCode
        });

        using (new AssertionScope())
        {
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should()
                .Contain(e => e.ErrorMessage.Contains($"'{isoCode}' is not found or invalid"));
        }
    }

    [Theory]
    [InlineData("01.15.2023", "01.14.2023")]
    [InlineData("01.15.2023", "02.16.2023")]
    public async Task GetReportModelValidatorTest_WrongDates_NegativeCase(string dateFrom, string dateTo)
    {
        _baseDataRepositoryMock.Setup(e => e.FindByCurrencyCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(new BaseCurrencyEntity());

        var validationResult = await _getReportModelValidator.ValidateAsync(new GetReportModel()
        {
            DateFrom = DateTime.Parse(dateFrom, CultureInfo.InvariantCulture),
            DateTo = DateTime.Parse(dateTo, CultureInfo.InvariantCulture),
            IsoCode = "AUD"
        });

        using (new AssertionScope())
        {
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
        }
    }

    [Fact]
    public async Task DateRangeValidatorTest_PositiveCase()
    {
        var validationResult = await _dateRangeValidator.ValidateAsync(new DateRangeModel()
        {
            DateFrom = DateTime.Parse("01.13.2023", CultureInfo.InvariantCulture),
            DateTo = DateTime.Parse("01.15.2023", CultureInfo.InvariantCulture),
        });

        using (new AssertionScope())
        {
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }
    }

    [Fact]
    public async Task DateRangeValidatorTest_WrongDates_NegativeTest()
    {
        var validationResult = await _dateRangeValidator.ValidateAsync(new DateRangeModel()
        {
            DateFrom = DateTime.Parse("01.14.2023", CultureInfo.InvariantCulture),
            DateTo = DateTime.Parse("01.13.2023", CultureInfo.InvariantCulture),
        });

        using (new AssertionScope())
        {
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should()
                .Contain(e => e.ErrorMessage.Contains("DateFrom must not be greater than DateTo"));
        }
    }

    [Fact]
    public async Task IsoCodeValidatorTest_PositiveTest()
    {
        // var isoCode = "AUD";
        _baseDataRepositoryMock.Setup(e => e.FindByCurrencyCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(new BaseCurrencyEntity());

        var validationResult = await _isoCodeValidator.ValidateAsync(new IsoCodeWrapperModel()
        {
            IsoCode = "AUD"
        });

        using (new AssertionScope())
        {
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }
    }

    [Fact]
    public async Task IsoCodeValidator_NegativeTest()
    {
        var isoCode = "AUD";
        _baseDataRepositoryMock.Setup(e => e.FindByCurrencyCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(null as BaseCurrencyEntity);

        var validationResult = await _isoCodeValidator.ValidateAsync(new IsoCodeWrapperModel()
        {
            IsoCode = isoCode
        });

        using (new AssertionScope())
        {
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should()
                .Contain(e => e.ErrorMessage.Contains($"'{isoCode}' is not found or invalid"));
        }
    }
}