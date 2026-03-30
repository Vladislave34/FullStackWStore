using Core.Interfaces;
using Core.Models.Size;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "StoreOwner, Admin")]
public class SizeController(ISizeService sizeService)  : ControllerBase
{
    [HttpGet("Sizes")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSizes()
    {
        var list = await sizeService.GetAllSizes();
        return Ok(list);
    }

    [HttpGet("Size/{id}")]
    public async Task<IActionResult> GetSizeById(Guid id)
    {
        var entity = await sizeService.GetSizeById(id);
        return Ok(entity);
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddSize(SizeAddUpdateModel model)
    {
        await sizeService.AddSize(model);
        return Ok();
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateSize(Guid id, SizeAddUpdateModel model)
    {
        await sizeService.UpdateSize(id, model);
        return Ok();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> RemoveSize(Guid id)
    {
        await sizeService.RemoveSize(id);
        return Ok();
    }

    [HttpDelete("RemoveAll")]
    public async Task<IActionResult> RemoveAllSizes()
    {
        await sizeService.RemoveAllSizes();
        return Ok();
    }
}