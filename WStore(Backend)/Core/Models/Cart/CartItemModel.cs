namespace Core.Models.Cart;

public class CartItemModel
{
    public Guid Id { get; set; }
    public List<Guid> CartItems { get; set; } = new List<Guid>();
}