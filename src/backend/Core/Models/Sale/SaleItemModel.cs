namespace Core.Models.Product.Sale;

public sealed record SaleItemModel
{
    public Guid Id { get; init; }
    public int Percent { get; init; }
}