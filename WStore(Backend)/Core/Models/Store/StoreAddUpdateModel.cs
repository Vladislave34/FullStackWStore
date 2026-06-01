using Core.Models.StoreImage;
using Microsoft.AspNetCore.Http;

namespace Core.Models.Store;

public class StoreAddUpdateModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<IFormFile> Images { get; set; } = new List<IFormFile>();
}