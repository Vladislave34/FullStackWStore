using Core.Interfaces;
using Core.Models.Product.Adrress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class AddressController(IAddressService addressService) : ControllerBase
{
    [HttpGet]
    
    public async Task<IActionResult> GetAddressesByUser()
    {
        var items = await addressService.GetAddressByUser();
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> AddAddress(AddressAddUpdateModel model)
    {
        await addressService.AddAddress(model);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAddress(Guid id, AddressAddUpdateModel model)
    {
        await addressService.UpdateAdrress(id, model);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAddress(Guid id)
    {
        await addressService.DeleteAdrress(id);
        return Ok();
    }
    
}