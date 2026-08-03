namespace Core.Models.Product.Adrress;

public sealed record AddressAddUpdateModel
{
    
    public string City { get; init; }
    public string Country { get; init; }
    public string Street { get; init; }
    public string HouseNumber { get; init; }
}