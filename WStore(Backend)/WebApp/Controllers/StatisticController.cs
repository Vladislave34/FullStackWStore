using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]/[action]")]
[ApiController]
public class StatisticController(IStatisticService statisticService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStatisticsByTime(int days)
    {
        var res = await statisticService.GetStatistics(days);
        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> GetStatisticsByCategory()
    {
        var res = await statisticService.GetStatisticsByCategory();
        return Ok(res);
    }
}