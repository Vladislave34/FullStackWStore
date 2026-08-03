namespace Core.Models.Product;

public sealed record ProductAddUpdateModel
{
    public string Name { get; init; }
    public string NameUk { get; init; }
    public string? Description { get; init; }
    public string? DescriptionUk { get; init; }
    public Guid GenderId { get; init; }
    public Guid CategoryId { get; init; }
    public Guid StoreId { get; init; }
}