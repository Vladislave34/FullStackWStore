using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace Core.Services;

// Core/Services/TelegramNotificationService.cs
public class TelegramNotificationService(IConfiguration config) : ITelegramNotificationService
{
    private readonly TelegramBotClient _bot = new(config["Telegram:BotToken"]!);

    public async Task SendOrderStatusAsync(long chatId, string orderStatus, Guid orderId)
    {
        var message = orderStatus switch
        {
            "Confirmed"  => $"✅ Замовлення #{orderId.ToString()[..8]} підтверджено",
            "Processing" => $"⚙️ Замовлення #{orderId.ToString()[..8]} в обробці",
            "Shipped"    => $"🚚 Замовлення #{orderId.ToString()[..8]} відправлено",
            "Delivered"  => $"📦 Замовлення #{orderId.ToString()[..8]} доставлено",
            "Cancelled"  => $"❌ Замовлення #{orderId.ToString()[..8]} скасовано",
            _            => $"ℹ️ Статус замовлення #{orderId.ToString()[..8]}: {orderStatus}"
        };

        await _bot.SendMessage(chatId, message);
    }

    public async Task SendMessageAsync(long chatId, string message)
    {
        await _bot.SendMessage(chatId, message);
    }
}