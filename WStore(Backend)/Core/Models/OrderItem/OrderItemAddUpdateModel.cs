namespace Core.Models.OrderItem;

public sealed record OrderItemAddUpdateModel
{
    public Guid ProductVariantId { get; init; }
    public Guid OrderId { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
}