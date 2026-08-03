namespace Core.Models.Order;

public sealed record OrderAddUpdateModel
{
    //public Guid CartId { get; init; }
    public List<Guid> CartItemIds { get; init; }
    public Guid AddressId { get; init; }
    public Guid PaymentId { get; init; }
}