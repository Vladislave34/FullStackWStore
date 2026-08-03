using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Order;
using Domain;
using Domain.Entities;
using Domain.Entities.Constants;
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

        
        

        

        
        var order = new OrderEntity
        {
            UserId = userId,
            Status = OrderStatus.Pending,
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

        

        
        var selectedItems = cart.Items.Where(x => model.CartItemIds.Contains(x.Id)).ToList();
        if (!selectedItems.Any())
            throw new Exception("No valid items selected");
        foreach (var cartItem in selectedItems)
            cartItem.IsDeleted = true;

        

        await context.SaveChangesAsync();

        
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
        if (order.Status is OrderStatus.Cancelled or OrderStatus.Delivered)
            throw new Exception("Cannot cancel this order");

        if (!Enum.TryParse<OrderStatus>(model.Status, ignoreCase: true, out var newStatus))
            throw new Exception($"Invalid status: {model.Status}");

        order.Status = newStatus;

        

        await context.SaveChangesAsync();

        if (order.User?.TelegramChatId != null)
        {
            await telegramService.SendOrderStatusAsync(
                order.User.TelegramChatId.Value,
                newStatus.ToString(),
                order.Id
            );
        }

        await redisService.RemoveAsync($"order:{id}");
        await redisService.RemoveByPrefixAsync("orders");
    }

    public async Task CancelOrder(Guid id)
    {
        var order = await context.Orders
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        

        if (order == null)
            throw new Exception("Order not found");
        if (order.Status is OrderStatus.Cancelled or OrderStatus.Delivered)
            throw new Exception("Cannot cancel this order");
        

        order.Status =  OrderStatus.Cancelled;

        

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
            //.Include(x => x.Status)
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
            //.Include(x => x.Status)
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
            //.Include(x => x.Status)
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
        if (store == null)
            throw new Exception("Store not found");
        string key = $"orders:store:{store.Id}";
        

        var cache = await redisService.GetAsync<List<OrderItemModel>>(key);
        if (cache != null) return cache;

        var items = await context.Orders
            .Where(x => !x.IsDeleted && x.Items.Any(i => 
                i.ProductVariant.Product.StoreId == store.Id))
            //.Include(x => x.Status)
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