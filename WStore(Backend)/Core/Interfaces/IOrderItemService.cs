using Core.Models.CartItem;
using Core.Models.OrderItem;

namespace Core.Interfaces;

public interface IOrderItemService
{
    Task AddOrderItem(OrderItemAddUpdateModel model);
    Task UpdateOrderItem(Guid id, OrderItemAddUpdateModel model);
    Task RemoveOrderItem(Guid id);
    Task RemoveAllOrderItems();
    
    Task<IEnumerable<OrderItemItemModel>> GetAllOrderItems();
    
    Task<OrderItemItemModel> GetOrderItemById(Guid id);
}