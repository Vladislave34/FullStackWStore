namespace Core.Models.Product;

public class ProductSearchModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string NameUk { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string DescriptionUk { get; set; } = null!;
    public Guid StoreId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid GenderId { get; set; }
    public List<ProductVariantSearchModel> Variants { get; set; } = new();
}

public class ProductVariantSearchModel
{
    public Guid ColorId { get; set; }
    public Guid SizeId { get; set; }
}