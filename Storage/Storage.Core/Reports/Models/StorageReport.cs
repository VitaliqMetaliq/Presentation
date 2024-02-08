namespace Storage.Core.Reports.Models;

public class StorageReport
{
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public string CurrencyCode { get; set; }
    public StorageReportModel[] Reports { get; set; }
}