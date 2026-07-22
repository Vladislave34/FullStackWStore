using Core.Interfaces;
using Core.Models.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[ApiController]
[Route("api/[controller]/[action]")]
//[Authorize(Roles = "StoreOwner, Admin")]
public class StoreController(IStoreService storeService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetSores()
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
    public async Task<IActionResult> GetStoreByUserId()
    {
        var store = await storeService.GetStoreByUserId();
        return Ok(store);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddStore([FromForm] StoreAddUpdateModel model)
    {
        var res = await storeService.AddStore(model);
        return Ok(res);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStore(Guid id, StoreAddUpdateModel model)
    {
        await storeService.UpdateStore(id, model);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStore(Guid id)
    {
        await storeService.RemoveStore(id);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveAllStores()
    {
        await storeService.RemoveAllStores();
        return Ok();
    }
    
    

}