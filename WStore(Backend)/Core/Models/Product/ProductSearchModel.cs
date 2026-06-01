namespace Core.Models.Product;

public class ProductSearchModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? NameUk { get; set; }
    public string? Description { get; set; }
    public string? DescriptionUk { get; set; }
    public string? CategoryName { get; set; }
    public string? CategoryNameUk { get; set; }
    public string? StoreName { get; set; }
    public decimal MinPrice { get; set; }
    public List<string> Colors { get; set; } = new();
    public List<string> Sizes { get; set; } = new();
}