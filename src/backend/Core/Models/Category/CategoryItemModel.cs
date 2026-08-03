namespace Core.Models.Category;

public sealed record CategoryItemModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string NameUk { get; init; } = string.Empty;
    public string Image { get; init; }
    
}