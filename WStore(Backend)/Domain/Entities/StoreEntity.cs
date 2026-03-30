using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Identity;

namespace Domain.Entities;

public class StoreEntity : BaseEntity<Guid>
{
    [Required] public string Name { get; set; } = null!;
    [Required] public string Description { get; set; } = null!;
    [ForeignKey(nameof(UserEntity))] public Guid OwnerId { get; set; }
    
    public UserEntity UserEntity { get; set; }
    
    public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    public ICollection<StoreImageEntity> Images { get; set; } = new List<StoreImageEntity>();

}