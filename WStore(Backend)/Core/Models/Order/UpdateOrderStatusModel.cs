namespace Core.Models.Order;

public sealed record UpdateOrderStatusModel
{
    public string Status { get; init; }
}