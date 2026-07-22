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
        var userId = await authService.GetUserIdAsync();

        
        var cart = await context.Carts
            .Include(x => x.Items)
            .ThenInclude(x => x.ProductVariant)
            .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);

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
            UserId = userId,
            OrderStatusId = pendingStatus.Id,
            TotalPrice = cart.Items
                .Where(x => model.CartItemIds.Contains(x.Id))
                .Sum(x => x.Price),
            Items = cart.Items.Where(x => model.CartItemIds.Contains(x.Id)).Select(x => new OrderItemEntity
            {
                ProductVariantId = x.ProductVariantId,
                Quantity = x.Quantity,
                Price = x.Price
            }).ToList(),
            AdrressId = model.AddressId,
            PaymentId = model.PaymentId
        };

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        // 4. записуємо в історію
        await context.Set<OrderHistoryEntity>().AddAsync(new OrderHistoryEntity
        {
            OrderId = order.Id,
            StatusId = pendingStatus.Id
        });

        // 5. очищаємо кошик (тільки вибрані товари)
        var selectedItems = cart.Items.Where(x => model.CartItemIds.Contains(x.Id)).ToList();
        foreach (var cartItem in selectedItems)
            cartItem.IsDeleted = true;

        // видаляємо сам кошик, тільки якщо всі товари з нього викуплені
        //if (cart.Items.All(x => x.IsDeleted))
        //    cart.IsDeleted = true;

        await context.SaveChangesAsync();

        // 6. кешуємо
        await redisService.RemoveByPrefixAsync("orders");
        await redisService.RemoveByPrefixAsync("order");
        string cacheKey = $"cartitems:all:{userId}";
        await redisService.RemoveAsync(cacheKey);
    }

    public async Task UpdateOrderStatus(Guid id, UpdateOrderStatusModel model)
    {
        var order = await context.Orders
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (order == null)
            throw new Exception("Order not found");

        var status = await context.OrderStatuses.FirstOrDefaultAsync(x => x.Name == model.Status);
        if (status == null)
            throw new Exception("Status not found");



        order.OrderStatusId = status.Id;

        // записуємо зміну в історію
        await context.Set<OrderHistoryEntity>().AddAsync(new OrderHistoryEntity
        {
            OrderId = order.Id,
            StatusId = status.Id
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
        await redisService.RemoveByPrefixAsync($"orders");
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
        await redisService.RemoveByPrefixAsync($"orders");
        
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
            .Include(x => x.Items)
                .ThenInclude(x => x.ProductVariant)
                    .ThenInclude(x => x.Image)
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
    public async Task<IEnumerable<OrderItemModel>> GetOrdersForStore()
    {
        var userId = await authService.GetUserIdAsync();
        var store = await context.Stores.FirstOrDefaultAsync(x => x.OwnerId == userId && !x.IsDeleted);
        string key = $"orders:store:{store.Id}";
        

        var cache = await redisService.GetAsync<List<OrderItemModel>>(key);
        if (cache != null) return cache;

        var items = await context.Orders
            .Where(x => !x.IsDeleted && x.UserId == store.OwnerId)
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
            .Include(x => x.Items)
            .ThenInclude(x => x.ProductVariant)
            .ThenInclude(x => x.Image)
            .ProjectTo<OrderItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        await redisService.SetAsync(key, items, TimeSpan.FromMinutes(10));
        return items;
    }
}