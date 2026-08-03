namespace Core.Models.Product.Gender;

public sealed record GenderItemModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string NameUk { get; init; } = string.Empty;
}