using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Order;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

// Core/Services/OrderService.cs
public class OrderService(
    AppStoreContext context,
    IRedisService redisService,
    IMapper mapper,
    ITelegramNotificationService telegramService,
    IAuthService authService) : IOrderService
{
    public async Task AddOrder(OrderAddUpdateModel model)
    {
        // 1. беремо кошик
        var cart = await context.Carts
            .Include(x => x.Items)
            .ThenInclude(x => x.ProductVariant)
            .FirstOrDefaultAsync(x => x.Id == model.CartId && !x.IsDeleted);

        if (cart == null)
            throw new Exception("Cart not found");

        if (!cart.Items.Any())
            throw new Exception("Cart is empty");

        // 2. статус Pending
        var pendingStatus = await context.OrderStatuses
            .FirstOrDefaultAsync(x => x.Name == "Pending");

        if (pendingStatus == null)
            throw new Exception("OrderStatus 'Pending' not found");

        // 3. створюємо замовлення
        var order = new OrderEntity
        {
            UserId = await authService.GetUserIdAsync(),
            OrderStatusId = pendingStatus.Id,
            TotalPrice = cart.Items.Sum(x => x.Price * x.Quantity),
            Items = cart.Items.Select(x => new OrderItemEntity
            {
                ProductVariantId = x.ProductVariantId,
                Quantity = x.Quantity,
                Price = x.Price
            }).ToList()
        };

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        // 4. записуємо в історію
        await context.Set<OrderHistoryEntity>().AddAsync(new OrderHistoryEntity
        {
            OrderId = order.Id,
            StatusId = pendingStatus.Id 
        });

        // 5. очищаємо кошик
        cart.IsDeleted = true;
        foreach (var cartItem in cart.Items)
            cartItem.IsDeleted = true;

        await context.SaveChangesAsync();

        // 6. кешуємо
        await redisService.RemoveAsync("orders:all");
    }

    public async Task UpdateOrderStatus(Guid id, UpdateOrderStatusModel model)
    {
        var order = await context.Orders
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (order == null)
            throw new Exception("Order not found");

        var status = await context.OrderStatuses.FindAsync(model.StatusId);
        if (status == null)
            throw new Exception("Status not found");

        order.OrderStatusId = model.StatusId;

        // записуємо зміну в історію
        await context.Set<OrderHistoryEntity>().AddAsync(new OrderHistoryEntity
        {
            OrderId = order.Id,
            StatusId = model.StatusId
        });

        await context.SaveChangesAsync();
        if (order.User?.TelegramChatId != null)
        {
            await telegramService.SendOrderStatusAsync(
                order.User.TelegramChatId.Value,
                status.Name,
                order.Id
            );
        }

        await redisService.RemoveAsync($"order:{id}");
        await redisService.RemoveAsync("orders:all");
    }

    public async Task CancelOrder(Guid id)
    {
        var order = await context.Orders
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (order == null)
            throw new Exception("Order not found");

        var cancelledStatus = await context.OrderStatuses
            .FirstOrDefaultAsync(x => x.Name == "Cancelled");

        if (cancelledStatus == null)
            throw new Exception("OrderStatus 'Cancelled' not found");

        order.OrderStatusId = cancelledStatus.Id;

        await context.Set<OrderHistoryEntity>().AddAsync(new OrderHistoryEntity
        {
            OrderId = order.Id,
            StatusId = cancelledStatus.Id
        });

        await context.SaveChangesAsync();

        await redisService.RemoveAsync($"order:{id}");
        await redisService.RemoveAsync("orders:all");
    }

    public async Task<IEnumerable<OrderItemModel>> GetAllOrders()
    {
        var cache = await redisService.GetAsync<List<OrderItemModel>>("orders:all");
        if (cache != null) return cache;

        var items = await context.Orders
            .Where(x => !x.IsDeleted)
            .Include(x => x.OrderStatus)
            .Include(x => x.Items)
                .ThenInclude(x => x.ProductVariant)
                    .ThenInclude(x => x.Product)
            .Include(x => x.Items)
                .ThenInclude(x => x.ProductVariant)
                    .ThenInclude(x => x.Color)
            .Include(x => x.Items)
                .ThenInclude(x => x.ProductVariant)
                    .ThenInclude(x => x.Size)
            .ProjectTo<OrderItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        await redisService.SetAsync("orders:all", items, TimeSpan.FromMinutes(10));
        return items;
    }

    public async Task<IEnumerable<OrderItemModel>> GetMyOrders()
    {
        var userId = await authService.GetUserIdAsync();
        string key = $"orders:user:{userId}";

        var cache = await redisService.GetAsync<List<OrderItemModel>>(key);
        if (cache != null) return cache;

        var items = await context.Orders
            .Where(x => !x.IsDeleted && x.UserId == userId)
            .Include(x => x.OrderStatus)
            .Include(x => x.Items)
                .ThenInclude(x => x.ProductVariant)
                    .ThenInclude(x => x.Product)
            .Include(x => x.Items)
                .ThenInclude(x => x.ProductVariant)
                    .ThenInclude(x => x.Color)
            .Include(x => x.Items)
                .ThenInclude(x => x.ProductVariant)
                    .ThenInclude(x => x.Size)
            .ProjectTo<OrderItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        await redisService.SetAsync(key, items, TimeSpan.FromMinutes(10));
        return items;
    }

    public async Task<OrderItemModel> GetOrderById(Guid id)
    {
        var cache = await redisService.GetAsync<OrderItemModel>($"order:{id}");
        if (cache != null) return cache;

        var entity = await context.Orders
            .Where(x => x.Id == id && !x.IsDeleted)
            .Include(x => x.OrderStatus)
            .Include(x => x.Items)
                .ThenInclude(x => x.ProductVariant)
                    .ThenInclude(x => x.Product)
            .Include(x => x.Items)
                .ThenInclude(x => x.ProductVariant)
                    .ThenInclude(x => x.Color)
            .Include(x => x.Items)
                .ThenInclude(x => x.ProductVariant)
                    .ThenInclude(x => x.Size)
            .FirstOrDefaultAsync();

        if (entity == null)
            throw new Exception("Order not found");

        var item = mapper.Map<OrderItemModel>(entity);
        await redisService.SetAsync($"order:{id}", item, TimeSpan.FromMinutes(10));
        return item;
    }
}