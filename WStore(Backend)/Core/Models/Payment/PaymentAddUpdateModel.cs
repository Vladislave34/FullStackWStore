namespace Core.Models.Product.Payment;

public class PaymentAddUpdateModel
{
    public string Number { get; set; }
    public string Date { get; set; }
    
    public string CVV { get; set; }
    public string OwnerName { get; set; }
    public string PaymentSystem { get; set; }
}