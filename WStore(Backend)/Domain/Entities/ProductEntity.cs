using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ProductEntity : BaseEntity<Guid>
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    
    
    [ForeignKey(nameof(CategoryEntity))]
    public Guid CategoryId { get; set; }
    
    
    [ForeignKey(nameof(StoreEntity))]
    public Guid StoreId { get; set; }

    
    
    
    
    public CategoryEntity CategoryEntity { get; set; }
    public ICollection<ProductVariantEntity> Variants { get; set; } = new List<ProductVariantEntity>();
    public ICollection<FeedbackEntity> Feedbacks { get; set; } = new List<FeedbackEntity>();
    
    public StoreEntity Store { get; set; }
}