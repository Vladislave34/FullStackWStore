using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Identity;

namespace Domain.Entities;

public class AdrressEntity : BaseEntity<Guid>
{
    [Required]
    public string City { get; set; }
    [Required]
    public string Country { get; set; }
    [Required]
    public string Street { get; set; }
    [Required]
    public string HouseNumber { get; set; }
    [ForeignKey(nameof(UserEntity))]
    public Guid? UserId { get; set; }
    public UserEntity? User { get; set; }
    
    public ICollection<OrderEntity> Orders { get; set; }
}