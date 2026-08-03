namespace Core.Models.Product.Adrress;

public sealed record AddressItemModel
{
    public Guid Id { get; init; }
    public string City { get; init; }
    public string Country { get; init; }
    public string Street { get; init; }
    public string HouseNumber { get; init; }
}