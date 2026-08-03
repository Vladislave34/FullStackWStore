using Core.Models.ProductVariant;
using Domain.Entities;

namespace Core.Models.Product;

public class ProductItemModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? NameUk { get; set; }
    public string? Description { get; set; }
    public string? DescriptionUk { get; set; }
    public string? Category { get; set; }
    
    public string? CategoryUk { get; set; }
    
    public string Gender { get; set; }
    public string GenderUk { get; set; }
    
    public string? Store { get; set; }
    public List<ProductVariantItemModel> Variants { get; set; } = new();
    public bool? IsFavourite { get; set; }

}