using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Identity;
using Telegram.Bot.Types;

namespace Domain.Entities;

public class ProductEntity : BaseEntity<Guid>
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string NameUk { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string DescriptionUk { get; set; }
    
    [ForeignKey(nameof(GenderEntity))]
    public Guid GenderId { get; set; }
    
    
    [ForeignKey(nameof(CategoryEntity))]
    public Guid CategoryId { get; set; }
    
    
    [ForeignKey(nameof(StoreEntity))]
    public Guid StoreId { get; set; }

    
    
    
    
    public CategoryEntity CategoryEntity { get; set; }
    public GenderEntity GenderEntity { get; set; }
    public ICollection<ProductVariantEntity> Variants { get; set; } = new List<ProductVariantEntity>();
    public ICollection<FeedbackEntity> Feedbacks { get; set; } = new List<FeedbackEntity>();
    
    public StoreEntity Store { get; set; }
    
    public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}