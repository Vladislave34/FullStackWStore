using Core.Models.Cart;
using Domain.Entities;

namespace Core.Models.CartItem;

public class CartItemItemModel
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public ProductDto Product { get; set; }
    public ProductVariantDto ProductVariant { get; set; }
}
public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string NameUk { get; set; }
    public string Description { get; set; }
    public string DescriptionUk { get; set; }
    
}

public class ProductVariantDto
{
    public Guid Id { get; set; }
    public string Color { get; set; }
    
    public string ColorUk { get; set; }
    public string Size { get; set; }
    public decimal Price { get; set; }
    public decimal Sale { get; set; }
    public List<string> Images { get; set; } = new();
    
}