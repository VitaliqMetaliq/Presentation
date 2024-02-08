using Storage.Core.Reports.Models;

namespace Storage.Core.Reports;

public interface IReportManager
{
    byte[] GetReport(StorageReport report);
    Task<StorageReport> MakeRubleReport(DateTime dateFrom, DateTime dateTo);
    Task<StorageReport> MakeReport(DateTime dateFrom, DateTime dateTo, string isoCode);
}
