using Core.Interfaces;
using Core.Models.Color;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "StoreOwner, Admin")]
public class ColorController(IColorService colorService) : ControllerBase
{
    [HttpGet("Colors")]
    [AllowAnonymous]
    public async Task<IActionResult> GetColors()
    {
        var list = await colorService.GetAllColors();
        return Ok(list);
    }

    [HttpGet("Color/{id}")]
    public async Task<IActionResult> GetColor(Guid id)
    {
        var entity = await colorService.GetColorById(id);
        return Ok(entity);
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddColor(ColorAddUpdateModel model)
    {
        await colorService.AddColor(model);
        return Ok();
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateColor(ColorAddUpdateModel model)
    {
        await colorService.UpdateColor(model);
        return Ok();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> RemoveColor(Guid id)
    {
        await colorService.RemoveColor(id);
        return Ok();
    }
    [HttpDelete("RemoveAll")]
    public async Task<IActionResult> RemoveAllColors()
    {
        await colorService.RemoveAllColors();
        return Ok();
    }
}