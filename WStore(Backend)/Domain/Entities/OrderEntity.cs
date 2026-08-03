using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Constants;
using Domain.Entities.Identity;

namespace Domain.Entities;

public class OrderEntity : BaseEntity<Guid>
{
    [Required] public decimal TotalPrice { get; set; }
    
    public OrderStatus Status { get; set; }
    
    [ForeignKey(nameof(UserEntity))] public Guid UserId { get; set; }
    [ForeignKey(nameof(PaymentEntity))] public Guid PaymentId { get; set; }
    [ForeignKey(nameof(AdrressEntity))] public Guid AdrressId { get; set; }
    
    public ICollection<OrderItemEntity> Items { get; set; } =  new List<OrderItemEntity>();
   
    public UserEntity User { get; set; }
    public PaymentEntity Payment { get; set; }
    public AdrressEntity Adrress { get; set; }

}