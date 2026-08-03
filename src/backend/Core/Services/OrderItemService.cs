using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.OrderItem;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class OrderItemService(AppStoreContext  context, IRedisService redisService, IMapper mapper) : IOrderItemService
{
    public async Task AddOrderItem(OrderItemAddUpdateModel model)
{
    var entity = mapper.Map<OrderItemEntity>(model);
    await context.OrderItems.AddAsync(entity);
    await context.SaveChangesAsync();

    var item = mapper.Map<OrderItemItemModel>(entity);
    await redisService.SetAsync($"orderitem:{item.Id}", item, TimeSpan.FromMinutes(10));
    await redisService.RemoveAsync("orderitems:all");
}

public async Task UpdateOrderItem(Guid id, OrderItemAddUpdateModel model)
{
    var entity = await context.OrderItems
        .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

    if (entity == null)
        throw new Exception("Item not found");

    mapper.Map(model, entity);
    await context.SaveChangesAsync();

    var item = mapper.Map<OrderItemItemModel>(entity);
    await redisService.SetAsync($"orderitem:{item.Id}", item, TimeSpan.FromMinutes(10));
    await redisService.RemoveAsync("orderitems:all");
}

public async Task RemoveOrderItem(Guid id)
{
    var entity = await context.OrderItems
        .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

    if (entity == null)
        throw new Exception("Item not found");

    entity.IsDeleted = true;
    await context.SaveChangesAsync();

    await redisService.RemoveAsync($"orderitem:{entity.Id}");
    await redisService.RemoveAsync("orderitems:all");
}

public async Task RemoveAllOrderItems()
{
    var entities = await context.OrderItems
        .Where(x => !x.IsDeleted)
        .ToListAsync();

    foreach (var entity in entities)
    {
        entity.IsDeleted = true;
        await redisService.RemoveAsync($"orderitem:{entity.Id}");
    }

    await redisService.RemoveAsync("orderitems:all");
    await context.SaveChangesAsync();
}

public async Task<IEnumerable<OrderItemItemModel>> GetAllOrderItems()
{
    var cache = await redisService.GetAsync<List<OrderItemItemModel>>("orderitems:all");
    if (cache != null)
        return cache;

    var items = await context.OrderItems
        .Where(x => !x.IsDeleted)
        .ProjectTo<OrderItemItemModel>(mapper.ConfigurationProvider)
        .ToListAsync();

    await redisService.SetAsync("orderitems:all", items, TimeSpan.FromMinutes(10));
    return items;
}

public async Task<OrderItemItemModel> GetOrderItemById(Guid id)
{
    var cache = await redisService.GetAsync<OrderItemItemModel>($"orderitem:{id}");
    if (cache != null)
        return cache;

    var entity = await context.OrderItems
        .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

    if (entity == null)
        throw new Exception("Item not found");

    var item = mapper.Map<OrderItemItemModel>(entity);
    await redisService.SetAsync($"orderitem:{id}", item, TimeSpan.FromMinutes(10));

    return item;
}
}