namespace Core.Models.Product;

public class ProductAddUpdateModel
{
    public string Name { get; set; }
    public string NameUk { get; set; }
    public string? Description { get; set; }
    public string? DescriptionUk { get; set; }
    public Guid CategoryId { get; set; }
    public Guid StoreId { get; set; }
}