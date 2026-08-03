using Core.Interfaces;
using Core.Models.OrderItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class OrderItemController(IOrderItemService orderItemService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOrderItems()
    {
        var list = await orderItemService.GetAllOrderItems();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderItemById(Guid id)
    {
        var entity = await orderItemService.GetOrderItemById(id);
        return Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrderItem(OrderItemAddUpdateModel model)
    {
        await orderItemService.AddOrderItem(model);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrderItem(Guid id, OrderItemAddUpdateModel model)
    {
        await orderItemService.UpdateOrderItem(id, model);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveOrderItem(Guid id)
    {
        await orderItemService.RemoveOrderItem(id);
        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveAllOrderItems()
    {
        await orderItemService.RemoveAllOrderItems();
        return Ok();
    }
}