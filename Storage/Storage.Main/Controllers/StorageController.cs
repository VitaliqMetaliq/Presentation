using MassTransit.Internals;
using Microsoft.AspNetCore.Mvc;
using Storage.Core.Reports;
using Storage.Main.Managers;
using Storage.Main.Models;

namespace Storage.Main.Controllers;

[ApiController]
[Route("[controller]")]
public class StorageController : ControllerBase
{
    private readonly IStorageManager _storageManager;
    private readonly IReportManager _reportManager;

    public StorageController(IStorageManager storageManager, IReportManager reportManager)
    {
        _storageManager = storageManager;
        _reportManager = reportManager;
    }

    [HttpGet]
    [Route("GetFilteredByDate")]
    [ProducesResponseType(typeof(DailyCurrencyViewModel[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IReadOnlyCollection<DailyCurrencyViewModel>> GetDataFilteredByDate([FromQuery] DateRangeModel model)
    {
        return await _storageManager.GetFilteredCurrenciesByDate(model.DateFrom, model.DateTo);
    }

    [HttpGet]
    [Route("GetFilteredByIsoCode")]
    [ProducesResponseType(typeof(DailyCurrencyViewModel[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IReadOnlyCollection<DailyCurrencyViewModel>> GetDataFilteredByIsoCode([FromQuery] IsoCodeWrapperModel model)
    {
        return await _storageManager.GetFilteredCurrenciesByIsoCode(model.IsoCode);
    }

    [HttpGet]
    [Route("DownloadRubReport")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public async Task<FileContentResult> GetReport([FromQuery] DateRangeModel model)
    {
        var generatedReport = await _reportManager
            .MakeRubleReport(
                DateTime.SpecifyKind(model.DateFrom, DateTimeKind.Utc),
                DateTime.SpecifyKind(model.DateTo, DateTimeKind.Utc));

        var report = _reportManager.GetReport(generatedReport);

        return File(report, "application/octet-stream", $"Report_RUB.csv");
    }

    [HttpGet]
    [Route("DownloadReport")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public async Task<FileContentResult> GetReport([FromQuery] GetReportModel model)
    {
        var generatedReport = await _reportManager
            .MakeReport(
                DateTime.SpecifyKind(model.DateFrom, DateTimeKind.Utc),
                DateTime.SpecifyKind(model.DateTo, DateTimeKind.Utc), model.IsoCode);

        var report = _reportManager.GetReport(generatedReport);

        return File(report, "application/octet-stream", $"Report_{model.IsoCode}.csv");
    }
}

