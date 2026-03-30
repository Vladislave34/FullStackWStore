using Core.Interfaces;
using Core.Models.CartItem;
using Core.Models.Category;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "StoreOwner, Admin")]
public class CartItemController(ICartItemService cartItemService) : ControllerBase
{
    [HttpGet("CartItems")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCartItems()
    {
        var list = await cartItemService.GetAllCartItems();
        return Ok(list);
    }

    [HttpGet("CartItem/{id}")]
    public async Task<IActionResult> GetCartItemById(Guid id)
    {
        var entity = await cartItemService.GetCartItemById(id);
        return Ok(entity);
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddCartItem(CartItemAddUpdateModel model)
    {
        await cartItemService.AddCartItem(model);
        return Ok();
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateCartItem(Guid id, CartItemAddUpdateModel model)
    {
        await cartItemService.UpdateCartItem(id, model);
        return Ok();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> RemoveCartItem(Guid id)
    {
        await cartItemService.RemoveCartItem(id);
        return Ok();
    }

    [HttpDelete("RemoveAll")]
    public async Task<IActionResult> RemoveAllCartItems()
    {
        await cartItemService.RemoveAllCartItems();
        return Ok();
    }

}