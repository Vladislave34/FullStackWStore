using Microsoft.AspNetCore.Http;

namespace Core.Models.Category;

public class CategoryAddUpdateModel
{
    public string Name { get; set; }
    
    public string NameUk { get; set; } = null!;
    public IFormFile Image { get; set; }
}