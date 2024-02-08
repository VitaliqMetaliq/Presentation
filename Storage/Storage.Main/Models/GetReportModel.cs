namespace Storage.Main.Models;

public class GetReportModel
{
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public string IsoCode { get; set; }
}