using Core.Interfaces;
using Core.Models.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class StoreController(IStoreService storeService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetStores()
    {
        var stores = await storeService.GetAllStores();
        return Ok(stores);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStoreById(Guid id)
    {
        var store = await storeService.GetStoreById(id);
        return Ok(store);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,StoreOwner")]
    public async Task<IActionResult> GetStoreByUserId()
    {
        var store = await storeService.GetStoreByUserId();
        return Ok(store);
    }

    [HttpPost]
    
    public async Task<IActionResult> AddStore([FromForm] StoreAddUpdateModel model)
    {
        var res = await storeService.AddStore(model);
        return Ok(res);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,StoreOwner")]
    public async Task<IActionResult> UpdateStore(Guid id, StoreAddUpdateModel model)
    {
        await storeService.UpdateStore(id, model);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,StoreOwner")]
    public async Task<IActionResult> DeleteStore(Guid id)
    {
        await storeService.RemoveStore(id);
        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveAllStores()
    {
        await storeService.RemoveAllStores();
        return Ok();
    }
    
    

}