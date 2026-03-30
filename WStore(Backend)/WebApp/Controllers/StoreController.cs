using Core.Interfaces;
using Core.Models.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "StoreOwner, Admin")]
public class StoreController(IStoreService storeService) : ControllerBase
{
    [HttpGet("Stores")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSores()
    {
        var stores = await storeService.GetAllStores();
        return Ok(stores);
    }

    [HttpGet("Store/{id}")]
    public async Task<IActionResult> GetStoreById(Guid id)
    {
        var store = await storeService.GetStoreById(id);
        return Ok(store);
    }

    [HttpPost("Add")]
    
    public async Task<IActionResult> AddStore([FromForm] StoreAddUpdateModel model)
    {
        await storeService.AddStore(model);
        return Ok();
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateStore(Guid id, StoreAddUpdateModel model)
    {
        await storeService.UpdateStore(id, model);
        return Ok();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteStore(Guid id)
    {
        await storeService.RemoveStore(id);
        return Ok();
    }

    [HttpDelete("RemoveAll")]
    public async Task<IActionResult> RemoveAllStores()
    {
        await storeService.RemoveAllStores();
        return Ok();
    }
    
    

}