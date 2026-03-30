namespace Domain.Entities;

public class SizeEntity : BaseEntity<Guid>
{
    public string Name { get; set; }
    public ICollection<ProductVariantEntity> ProductVariants { get; set; } = new List<ProductVariantEntity>();
}