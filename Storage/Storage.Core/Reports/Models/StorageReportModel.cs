namespace Storage.Core.Reports.Models;

public class StorageReportModel
{
    public string CurrencyName { get; set; }

    public ReportItemModel[] Items { get; set; }
}