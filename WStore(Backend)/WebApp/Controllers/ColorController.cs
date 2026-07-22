using Core.Interfaces;
using Core.Models.Color;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = "StoreOwner, Admin")]
public class ColorController(IColorService colorService) : ControllerBase
{
    private string Lang =>
        Request.Headers["Accept-Language"].FirstOrDefault() ?? "en";
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetColors()
    {
        var list = await colorService.GetAllColors(Lang);
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetColor(Guid id)
    {
        var entity = await colorService.GetColorById(id, Lang);
        return Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> AddColor(ColorAddUpdateModel model)
    {
        await colorService.AddColor(model);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateColor(ColorAddUpdateModel model)
    {
        await colorService.UpdateColor(model);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveColor(Guid id)
    {
        await colorService.RemoveColor(id);
        return Ok();
    }
    [HttpDelete]
    public async Task<IActionResult> RemoveAllColors()
    {
        await colorService.RemoveAllColors();
        return Ok();
    }
}