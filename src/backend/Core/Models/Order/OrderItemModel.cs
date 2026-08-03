using Core.Models.OrderItem;
using Core.Models.Product.Payment;

namespace Core.Models.Order;

public class OrderItemModel
{
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }
    public string OrderStatus { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Address { get; set; }
    public PaymentItemModel Payment { get; set; }
    public ICollection<OrderItemItemModel> Items { get; set; } = new List<OrderItemItemModel>();
}