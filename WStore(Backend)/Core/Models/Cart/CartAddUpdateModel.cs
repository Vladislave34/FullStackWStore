namespace Core.Models.Cart;

public class CartAddUpdateModel
{
    public List<Guid> Cartitems { get; set; } = new List<Guid>();
}