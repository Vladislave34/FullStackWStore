namespace Core.Models.Cart;

public sealed record CartAddUpdateModel
{
    public List<Guid> Cartitems { get; init; } = new List<Guid>();
}