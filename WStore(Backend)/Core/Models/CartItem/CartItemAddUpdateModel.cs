namespace Core.Models.CartItem;

public class CartItemAddUpdateModel
{
    public Guid CartId { get; set; }
    public Guid ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}