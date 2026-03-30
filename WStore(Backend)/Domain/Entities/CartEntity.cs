using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Identity;

namespace Domain.Entities;

public class CartEntity : BaseEntity<Guid>
{
    [ForeignKey("UserEntity")]
    public Guid UserId { get; set; }
    public  UserEntity User { get; set; }
    public ICollection<CartItemEntity>? Items { get; set; } = new List<CartItemEntity>();
}