using TestAccruent.Controllers;
using TestAccruent.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using TestAccruent.Model;

namespace TestAccruent.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;
    private readonly ILogger<StockController> _logger;
    public StockController(IStockService stockService, ILogger<StockController> logger)
    {
        _stockService = stockService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] bool? isActive = null)
    {
        var stock = await _stockService.GetAllStocks(isActive);
        return Ok(stock);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> Get(int id)
    {
        var stock = await _stockService.GetStockByID(id);
        if (stock == null)
        {
            return NotFound();
        }
        return Ok(stock);
    }

    [Route("/api/stock/validate/{filter}")]
    [HttpGet()]
    [Authorize]
    public async Task<IActionResult> ValidateNegativeStockForProduct(string filter)
    {
        var data = filter.Split(';');

        var valide = await _stockService.ValidateNegativeStockForProduct(data[0], Convert.ToInt32(data[1]));
        return Ok(valide);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] Stock stockObject)
    {
        var stock = await _stockService.AddStock(stockObject);

        if (stock == null)
        {
            return BadRequest();
        }

        return Ok(new
        {
            message = "Created Successfully!!!",
            id = stock!.Id
        });
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Stock stockObject)
    {
        var stock = await _stockService.UpdateStock(id, stockObject);
        if (stock == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            message = "Updated Successfully!!!",
            id = stock!.Id
        });
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!await _stockService.DeleteStockByID(id))
        {
            return NotFound();
        }

        return Ok(new
        {
            message = "Deleted Successfully!!!",
            id = id
        });
    }
}
