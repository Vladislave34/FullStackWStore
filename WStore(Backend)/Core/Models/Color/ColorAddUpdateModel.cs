namespace Core.Models.Color;

public sealed record ColorAddUpdateModel
{
    public string Name { get; init; }
    public string NameUk { get; init; }
    public string Hex { get; init; }
    
}