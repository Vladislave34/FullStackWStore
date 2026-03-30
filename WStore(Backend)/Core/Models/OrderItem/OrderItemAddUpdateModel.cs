namespace Core.Models.OrderItem;

public class OrderItemAddUpdateModel
{
    public Guid ProductVariantId { get; set; }
    public Guid OrderId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}