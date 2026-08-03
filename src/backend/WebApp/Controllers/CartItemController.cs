using Core.Interfaces;
using Core.Models.CartItem;
using Core.Models.Category;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class CartItemController(ICartItemService cartItemService) : ControllerBase
{
    private string Lang =>
        Request.Headers["Accept-Language"].FirstOrDefault() ?? "en";
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetCartItems()
    {
        var list = await cartItemService.GetAllCartItems();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCartItemById(Guid id)
    {
        var entity = await cartItemService.GetCartItemById(id);
        return Ok(entity);
    }

    [HttpPost]
    
    public async Task<IActionResult> AddCartItem(CartItemAddUpdateModel model)
    {
        await cartItemService.AddCartItem(model);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCartItem(Guid id, CartItemAddUpdateModel model)
    {
        await cartItemService.UpdateCartItem(id, model);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveCartItem(Guid id)
    {
        await cartItemService.RemoveCartItem(id);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveAllCartItems()
    {
        await cartItemService.RemoveAllCartItems();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCartItemsByUser()
    {
        var list = await cartItemService.GetCartItemsByUser();
        return Ok(list);
    }

    

}