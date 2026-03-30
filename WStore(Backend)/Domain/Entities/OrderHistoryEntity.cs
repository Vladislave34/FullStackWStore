using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class OrderHistoryEntity : BaseEntity<Guid>
{
    [ForeignKey(nameof(OrderEntity))]
    
    public Guid OrderId { get; set; }
    public OrderEntity Order { get; set; } 
    
    [ForeignKey(nameof(OrderStatusEntity))]
    public Guid StatusId { get; set; }
    public OrderStatusEntity Status { get; set; }
}