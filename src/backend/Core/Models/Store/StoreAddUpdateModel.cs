using Core.Models.StoreImage;
using Microsoft.AspNetCore.Http;

namespace Core.Models.Store;

public sealed record StoreAddUpdateModel
{
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public List<IFormFile> Images { get; init; } = new List<IFormFile>();
}