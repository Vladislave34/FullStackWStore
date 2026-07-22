namespace Core.Models.OrderItem;

public class OrderItemItemModel
{
    public Guid Id { get; set; }
    public Guid ProductVariantId { get; set; }
    public string ProductName { get; set; }
    
    public string ProductNameUk { get; set; }
    public string ColorName { get; set; }
    public string ColorNameUk { get; set; }
    public string SizeName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public ICollection<string> Images { get; set; }
}