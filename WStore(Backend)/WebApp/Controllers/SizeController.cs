using Core.Interfaces;
using Core.Models.Size;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = "StoreOwner, Admin")]
public class SizeController(ISizeService sizeService)  : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetSizes()
    {
        var list = await sizeService.GetAllSizes();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSizeById(Guid id)
    {
        var entity = await sizeService.GetSizeById(id);
        return Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> AddSize(SizeAddUpdateModel model)
    {
        await sizeService.AddSize(model);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSize(Guid id, SizeAddUpdateModel model)
    {
        await sizeService.UpdateSize(id, model);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveSize(Guid id)
    {
        await sizeService.RemoveSize(id);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveAllSizes()
    {
        await sizeService.RemoveAllSizes();
        return Ok();
    }
}