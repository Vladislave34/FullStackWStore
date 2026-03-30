using Core.Interfaces;
using Core.Models.Cart;
using Core.Models.CartItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "StoreOwner, Admin")]
public class CartController(ICartService cartService) : ControllerBase
{
    [HttpGet("Carts")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCarts()
    {
        var list = await cartService.GetAllCarts();
        return Ok(list);
    }

    [HttpGet("Cart/{id}")]
    public async Task<IActionResult> GetCartById(Guid id)
    {
        var entity = await cartService.GetCartById(id);
        return Ok(entity);
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddCart()
    {
        await cartService.AddCart();
        return Ok();
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateCart(Guid id, CartAddUpdateModel model)
    {
        await cartService.UpdateCart(id, model);
        return Ok();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> RemoveCart(Guid id)
    {
        await cartService.RemoveCart(id);
        return Ok();
    }

    [HttpDelete("RemoveAll")]
    public async Task<IActionResult> RemoveAllCarts()
    {
        await cartService.RemoveAllCarts();
        return Ok();
    }
}