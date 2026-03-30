using Core.Interfaces;
using Core.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;



[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] OrderAddUpdateModel model)
    {
        await orderService.AddOrder(model);
        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,StoreOwner")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusModel model)
    {
        await orderService.UpdateOrderStatus(id, model);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        await orderService.CancelOrder(id);
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await orderService.GetAllOrders();
        return Ok(orders);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyOrders()
    {
        var orders = await orderService.GetMyOrders();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var order = await orderService.GetOrderById(id);
        return Ok(order);
    }
}