using Core.Interfaces;
using Core.Models.Cart;
using Core.Models.CartItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class CartController(ICartService cartService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetCarts()
    {
        var list = await cartService.GetAllCarts();
        return Ok(list);
    }

    [HttpGet]
    public async Task<IActionResult> GetCartById(Guid id)
    {
        var entity = await cartService.GetCartById(id);
        return Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> AddCart()
    {
        var id = await cartService.AddCart();
        return Ok(id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCart(Guid id, CartAddUpdateModel model)
    {
        await cartService.UpdateCart(id, model);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveCart(Guid id)
    {
        await cartService.RemoveCart(id);
        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveAllCarts()
    {
        await cartService.RemoveAllCarts();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> HasCart()
    {
        bool hasCart = await cartService.HasCart();
        return Ok(hasCart);
    }

    [HttpGet]
    public async Task<IActionResult> GetCartByUser()
    {
        var item = await cartService.GetCartItemByUser();
        return Ok(item);
    }
}