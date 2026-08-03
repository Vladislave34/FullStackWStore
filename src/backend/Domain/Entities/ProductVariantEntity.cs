using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ProductVariantEntity : BaseEntity<Guid>
{
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; }

    public Guid ColorId { get; set; }
    public ColorEntity Color { get; set; }

    public Guid SizeId { get; set; }
    public SizeEntity Size { get; set; }
    
    [ForeignKey(nameof(SaleEntity))]
    public Guid? SaleId { get; set; }
    public SaleEntity? Sale { get; set; }

    
    public decimal Price { get; set; }
    public ICollection<ProductVariantImageEntity> Image { get; set; } =  new List<ProductVariantImageEntity>();
    public ICollection<ProductVariantEntity> Variants { get; set; } =  new List<ProductVariantEntity>();
    public ICollection<CartItemEntity> CartItems { get; set; } = new List<CartItemEntity>();
}