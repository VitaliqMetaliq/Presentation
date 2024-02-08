using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Storage.Core.Caching;
using Storage.Core.Caching.Models;
using Storage.Core.Mapping;
using Storage.Core.Reports.Models;
using Storage.Database.Entities;
using Storage.Database.Repository;
using Storage.Database.Specifications;

namespace Storage.Core.Reports;

public class ReportManager : IReportManager
{
    private readonly IDailyDataRepository _dailyDataRepository;
    private readonly ICacheService _cache;

    public ReportManager(IDailyDataRepository dailyDataRepository, ICacheService cache)
    {
        _dailyDataRepository = dailyDataRepository;
        _cache = cache;
    }

    public byte[] GetReport(StorageReport report)
    {
        var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("Storage Report");
        int columnsCount = (report.DateTo.AddDays(1) - report.DateFrom).Days + 2;
        // configure header
        ConfigureHeaderCells(sheet.Cells[1, 1, 2, columnsCount],
            $"Курсы валют по отношению к {report.CurrencyCode} в период с {report.DateFrom:dd/MM/yyyy} по {report.DateTo:dd/MM/yyyy}");

        // configure column headers cells
        ConfigureColumnHeaderCells(sheet, report.DateFrom, columnsCount);

        // configure data cells
        ConfigureDataCells(sheet, report.Reports, report.DateFrom, columnsCount);

        sheet.View.FreezePanes(1, 2);
        return package.GetAsByteArray();
    }

    public async Task<StorageReport> MakeRubleReport(DateTime dateFrom, DateTime dateTo)
    {
        var currencyList = new List<DailyCurrencyEntity>();
        for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
        {
            var cacheEntry = _cache.Get<DailyCurrencyCacheModel[]>(date.ToString("dd/MM/yyyy"));
            if (cacheEntry is { Length: > 0 })
            {
                currencyList.AddRange(cacheEntry.Select(e => e.ToEntity()));
            }
            else
            {
                var currencies = await _dailyDataRepository.GetFilteredItems(new DateIsoCodeSpecifications(date, date, "RUB"));
                if (currencies.Count() != 0)
                {
                    _cache.Set<DailyCurrencyCacheModel[]>(date.ToString("dd/MM/yyyy"), currencies.Select(e => e.ToModel()).ToArray());
                    currencyList.AddRange(currencies);
                }
            }
        }

        var reportItems = this.GroupReports(currencyList);

        return new StorageReport()
        {
            DateFrom = dateFrom,
            DateTo = dateTo,
            CurrencyCode = "RUB",
            Reports = reportItems
        };
    }

    public async Task<StorageReport> MakeReport(DateTime dateFrom, DateTime dateTo, string isoCode)
    {
        var currencies = await _dailyDataRepository
            .GetFilteredItems(new DateIsoCodeSpecifications(dateFrom, dateTo, isoCode));

        var reportItems = this.GroupReports(currencies);

        return new StorageReport()
        {
            DateFrom = dateFrom,
            DateTo = dateTo,
            CurrencyCode = isoCode,
            Reports = reportItems
        };
    }

    private StorageReportModel[] GroupReports(IEnumerable<DailyCurrencyEntity> currencies)
    {
        return currencies.GroupBy(x => x.Currency.Name)
            .Select(e => new StorageReportModel()
            {
                CurrencyName = e.Key,
                Items = e.Select(c => new ReportItemModel()
                {
                    Date = c.Date,
                    Value = c.Value
                }).OrderBy(o => o.Date).ToArray()
            }).ToArray();
    }

    private void ConfigureDataCells(ExcelWorksheet sheet, StorageReportModel[] reports, DateTime dateFrom,
        int columnsCount)
    {
        if (reports.Length > 0)
        {
            int row = 4;
            for (var iterator = reports.GetEnumerator(); iterator.MoveNext(); row++)
            {
                var report = iterator.Current as StorageReportModel;
                DateTime date = dateFrom;
                sheet.Cells[row, 1].Value = report.CurrencyName;
                sheet.Cells[row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(Color.DarkSeaGreen);
                for (int column = 2; column < columnsCount; column++)
                {
                    foreach (var item in report.Items)
                    {
                        sheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(Color.Bisque);
                        if (item.Date == date)
                        {
                            sheet.Cells[row, column].Value = Math.Round(item.Value, 2);
                        }
                    }

                    date = date.AddDays(1);
                }
            }
            var lastRow = reports.Length + 3;
            sheet.Cells[4, 1, lastRow, 1].AutoFitColumns();

            // calculate average value
            for (int i = 4; i <= lastRow; i++)
            {
                sheet.Cells[i, columnsCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[i, columnsCount].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                sheet.Cells[i, columnsCount].Formula =
                    $"=ROUND(AVERAGE({sheet.Cells[i, 2].Address}:{sheet.Cells[i, columnsCount - 1]}),2)";
            }
        }
    }

    private void ConfigureColumnHeaderCells(ExcelWorksheet sheet, DateTime dateFrom, int count)
    {
        int i = 1;
        DateTime date = dateFrom;
        sheet.Cells[3, i].Value = "Название валюты";
        while (++i < count)
        {
            sheet.Cells[3, i].Value = date.ToString("dd/MM/yyyy");
            sheet.Cells[3, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[3, i].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            date = date.AddDays(1);
        }
        sheet.Cells[3, i].Value = "Среднее значение";
        sheet.Cells[3, 1, 3, count].AutoFitColumns();
    }

    private void ConfigureHeaderCells(ExcelRange range, string cellValue)
    {
        range.Merge = true;
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(Color.Bisque);
        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        range.Value = cellValue;
    }
}
