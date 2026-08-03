namespace Core.Models.Color;

public sealed record ColorItemModel
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string NameUk { get; init; }
    public string Hex { get; init; }
}