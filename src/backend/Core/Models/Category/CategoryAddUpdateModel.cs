using Microsoft.AspNetCore.Http;

namespace Core.Models.Category;

public sealed record CategoryAddUpdateModel
{
    public string Name { get; init; }
    
    public string NameUk { get; init; } = null!;
    public IFormFile Image { get; init; }
}