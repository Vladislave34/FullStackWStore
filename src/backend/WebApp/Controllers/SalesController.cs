using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]/[action]")]
[ApiController]
public class SalesController(ISaleService saleService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSales()
    {
        var sales = await saleService.GetSales();
        return Ok(sales);
    }
}