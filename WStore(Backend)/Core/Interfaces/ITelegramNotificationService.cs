namespace Core.Interfaces;

public interface ITelegramNotificationService
{
    Task SendOrderStatusAsync(long chatId, string orderStatus, Guid orderId);
    Task SendMessageAsync(long chatId, string message);
}