namespace Domain.Entities;

public class OrderStatusEntity : BaseEntity<Guid>
{
    public string Name { get; set; }
    
    public ICollection<OrderEntity> Order { get; set; }  =   new List<OrderEntity>();
}
/*public enum OrderStatus
{
    Pending = 0,
    Paid = 1,
    Shipped = 2,
    Delivered = 3,
    Cancelled = 4
}*/