namespace Core.Models.CartItem;

public sealed record CartItemAddUpdateModel
{
    public Guid CartId { get; init; }
    public Guid ProductVariantId { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
}