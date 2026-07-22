namespace Core.Models.Order;

public class OrderAddUpdateModel
{
    //public Guid CartId { get; set; }
    public List<Guid> CartItemIds { get; set; }
    public Guid AddressId { get; set; }
    public Guid PaymentId { get; set; }
}