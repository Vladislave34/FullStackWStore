using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Identity;

namespace Domain.Entities;

public class OrderEntity : BaseEntity<Guid>
{
    [Required] public decimal TotalPrice { get; set; }
    [ForeignKey(nameof(OrderStatusEntity))]
    public Guid OrderStatusId { get; set; }
    [ForeignKey(nameof(UserEntity))] public Guid UserId { get; set; }
    
    public ICollection<OrderItemEntity> Items { get; set; } =  new List<OrderItemEntity>();
    public OrderStatusEntity OrderStatus { get; set; }
    public UserEntity User { get; set; }
    public ICollection<OrderHistoryEntity> Histories { get; set; } = new List<OrderHistoryEntity>();
}