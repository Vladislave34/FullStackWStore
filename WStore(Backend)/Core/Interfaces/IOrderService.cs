using Core.Models.Cart;
using Core.Models.Order;

namespace Core.Interfaces;

public interface IOrderService
{
    Task AddOrder(OrderAddUpdateModel model);
    Task UpdateOrderStatus(Guid id, UpdateOrderStatusModel model);
    Task CancelOrder(Guid id);
    Task<IEnumerable<OrderItemModel>> GetAllOrders();
    Task<IEnumerable<OrderItemModel>> GetMyOrders();
    Task<OrderItemModel> GetOrderById(Guid id);
}