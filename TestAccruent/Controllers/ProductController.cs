using TestAccruent.Controllers;
using TestAccruent.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using TestAccruent.Model;

namespace TestAccruent.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;
    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        var result = await _productService.Get();
        return Ok(result);
    }

    [Route("/api/product/validate/{filter}")]
    [HttpGet()]
    [Authorize]
    public async Task<IActionResult> ValidateIfProductExist(string filter)
    {
        var valide = await _productService.ValidateIfProductExist(filter);
        return Ok(valide);
    }

}
