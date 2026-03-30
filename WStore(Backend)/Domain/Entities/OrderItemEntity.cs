using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class OrderItemEntity : BaseEntity<Guid>
{
    [ForeignKey(nameof(ProductVariantEntity))]
    public Guid ProductVariantId { get; set; }
    public ProductVariantEntity ProductVariant { get; set; }
    [ForeignKey(nameof(OrderEntity))]
    public Guid OrderId { get; set; }
    public OrderEntity Order { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}