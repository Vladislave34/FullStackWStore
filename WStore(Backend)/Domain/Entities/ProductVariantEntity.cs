namespace Domain.Entities;

public class ProductVariantEntity : BaseEntity<Guid>
{
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; }

    public Guid ColorId { get; set; }
    public ColorEntity Color { get; set; }

    public Guid SizeId { get; set; }
    public SizeEntity Size { get; set; }
    public CartItemEntity CartItem { get; set; }

    
    public decimal Price { get; set; }
    public ICollection<ProductVariantImageEntity> Image { get; set; } =  new List<ProductVariantImageEntity>();
}