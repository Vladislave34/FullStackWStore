using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class CartItemEntity: BaseEntity<Guid>
{
    [ForeignKey(nameof(CartEntity))]
    public Guid CartId { get; set; }
    public CartEntity Cart { get; set; }
    
    [ForeignKey(nameof(ProductVariantEntity))]
    public Guid ProductVariantId { get; set; }
    public ProductVariantEntity ProductVariant { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; } 

}