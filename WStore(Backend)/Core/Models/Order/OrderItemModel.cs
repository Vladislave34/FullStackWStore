using Core.Models.OrderItem;

namespace Core.Models.Order;

public class OrderItemModel
{
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }
    public string OrderStatus { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<OrderItemItemModel> Items { get; set; } = new List<OrderItemItemModel>();
}