using Storage.Core.Reports;
using Storage.Database.Entities;
using Storage.Database.Repository;
using Storage.Database.Specifications;

namespace Storage.Tests;

public class ReportManagerTests
{
    private readonly Mock<IDailyDataRepository> _dailyDataRepositoryMock = new();
    private readonly IReportManager _reportManager;

    public ReportManagerTests()
    {
        _reportManager = new ReportManager(_dailyDataRepositoryMock.Object);
    }

    [Fact]
    public async Task MakeReportTest_PositiveCase()
    {
        #region Arrange

        var dateFrom = DateTime.Now.AddDays(-1);
        var dateTo = DateTime.Now;

        var baseCurrency = new BaseCurrencyEntity()
        {
            ISOCharCode = "AUD",
            EngName = "AUD_EngName",
            Name = "AUD_Name",
            Id = 1,
            ParentCode = "ParentCodeAud"
        };

        var firstDailyCurrency = new DailyCurrencyEntity()
        {
            BaseCurrency = baseCurrency,
            BaseCurrencyId = baseCurrency.Id,
            Currency = new BaseCurrencyEntity()
            {
                ISOCharCode = "USD",
                EngName = "USD_EngName",
                Name = "USD_Name",
                Id = 2,
                ParentCode = "ParentCodeUsd"
            },
            CurrencyId = 2,
            Id = 1,
            Value = 1.11d,
            Date = DateTime.Today
        };

        var secondDailyCurrency = new DailyCurrencyEntity()
        {
            BaseCurrency = baseCurrency,
            BaseCurrencyId = baseCurrency.Id,
            Currency = new BaseCurrencyEntity()
            {
                ISOCharCode = "DKK",
                EngName = "DKK_EngName",
                Name = "DKK_Name",
                Id = 3,
                ParentCode = "ParentCodeDkk"
            },
            CurrencyId = 3,
            Id = 2,
            Value = 2.22d,
            Date = DateTime.Today
        };

        var thirdDailyCurrency = new DailyCurrencyEntity()
        {
            BaseCurrency = baseCurrency,
            BaseCurrencyId = baseCurrency.Id,
            Currency = new BaseCurrencyEntity()
            {
                ISOCharCode = "GBP",
                EngName = "GBP_EngName",
                Name = "GBP_Name",
                Id = 4,
                ParentCode = "ParentCodeGbp"
            },
            CurrencyId = 4,
            Id = 3,
            Value = 3.33d,
            Date = DateTime.Today
        };

        var dailyCurrencies = new List<DailyCurrencyEntity>()
        {
            firstDailyCurrency, secondDailyCurrency, thirdDailyCurrency
        };

        _dailyDataRepositoryMock.Setup(e => e.GetFilteredItems(It.IsAny<DateIsoCodeSpecifications>()))
            .ReturnsAsync(dailyCurrencies);

        #endregion

        var report = await _reportManager.MakeReport(dateFrom, dateTo, baseCurrency.ISOCharCode);

        using (new AssertionScope())
        {
            report.CurrencyCode.Should().Be(baseCurrency.ISOCharCode);
            report.DateFrom.Should().Be(dateFrom);
            report.DateTo.Should().Be(dateTo);
            report.Reports.Should().NotBeEmpty();
            report.Reports.Length.Should().Be(3);
            report.Reports.Should().OnlyContain(e => e.CurrencyName == firstDailyCurrency.Currency.Name
                                                     || e.CurrencyName == secondDailyCurrency.Currency.Name
                                                     || e.CurrencyName == thirdDailyCurrency.Currency.Name);
            foreach (var reportModel in report.Reports)
            {
                reportModel.Items.Should().NotBeEmpty();
                reportModel.Items.Should().HaveCount(1);
                reportModel.Items.Should().OnlyContain(e => e.Value == firstDailyCurrency.Value
                                                            || e.Value == secondDailyCurrency.Value
                                                            || e.Value == thirdDailyCurrency.Value);
            }
        }
    }

    [Fact]
    public async Task MakeReportTest_NoData_PositiveCase()
    {
        var dateFrom = DateTime.Now.AddDays(-1);
        var dateTo = DateTime.Now;
        var isoCode = "Whatever";
        _dailyDataRepositoryMock.Setup(e => e.GetFilteredItems(It.IsAny<DateIsoCodeSpecifications>()))
            .ReturnsAsync(Array.Empty<DailyCurrencyEntity>());

        var report = await _reportManager.MakeReport(dateFrom, dateTo, isoCode);

        using (new AssertionScope())
        {
            report.Should().NotBeNull();
            report.DateFrom.Should().Be(dateFrom);
            report.DateTo.Should().Be(dateTo);
            report.CurrencyCode.Should().Be(isoCode);
            report.Reports.Should().BeEmpty();
        }
    }
}