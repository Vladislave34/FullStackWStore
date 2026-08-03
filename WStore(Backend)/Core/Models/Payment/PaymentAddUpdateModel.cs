namespace Core.Models.Product.Payment;

public sealed record PaymentAddUpdateModel
{
    public string Number { get; init; }
    public string Date { get; init; }
    
    public string CVV { get; init; }
    public string OwnerName { get; init; }
    public string PaymentSystem { get; init; }
}