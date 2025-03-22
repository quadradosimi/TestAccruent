using TestAccruent.Controllers;
using TestAccruent.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using TestAccruent.Model;

namespace TestAccruent.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly ILogger<ReportController> _logger;
    public ReportController(IReportService reportService, ILogger<ReportController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    [HttpGet("{data}")]
    [Authorize]
    public async Task<IActionResult> GetStockBalance(string? data)
    {

        if (data == "1")
        {
            return Ok(await _reportService.GetStockBalance());
        }
        else
        {
            ReportStockBalanceByProductRequest filter = new ReportStockBalanceByProductRequest();

            var a = data.Split(';');

            filter.MovementDate = Convert.ToDateTime(a[0]);
            filter.ProductId = a[1];

            return Ok(await _reportService.GetStockBalanceFilter(filter));

        }
           

    }

}
