using Core.Interfaces;
using Core.Models.OrderItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]")]
[ApiController]
public class OrderItemController(IOrderItemService orderItemService) : ControllerBase
{
    [HttpGet("OrderItems")]
    [AllowAnonymous]
    public async Task<IActionResult> GetOrderItems()
    {
        var list = await orderItemService.GetAllOrderItems();
        return Ok(list);
    }

    [HttpGet("OrderItem/{id}")]
    public async Task<IActionResult> GetOrderItemById(Guid id)
    {
        var entity = await orderItemService.GetOrderItemById(id);
        return Ok(entity);
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddOrderItem(OrderItemAddUpdateModel model)
    {
        await orderItemService.AddOrderItem(model);
        return Ok();
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateOrderItem(Guid id, OrderItemAddUpdateModel model)
    {
        await orderItemService.UpdateOrderItem(id, model);
        return Ok();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> RemoveOrderItem(Guid id)
    {
        await orderItemService.RemoveOrderItem(id);
        return Ok();
    }

    [HttpDelete("RemoveAll")]
    public async Task<IActionResult> RemoveAllOrderItems()
    {
        await orderItemService.RemoveAllOrderItems();
        return Ok();
    }
}