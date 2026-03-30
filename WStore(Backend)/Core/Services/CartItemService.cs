using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.CartItem;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class CartItemService(AppStoreContext  context, IRedisService redisService, IMapper mapper) : ICartItemService
{
    public async Task AddCartItem(CartItemAddUpdateModel model)
    {
        var entity = mapper.Map<CartItemEntity>(model);
        await context.CartItems.AddAsync(entity);
        await context.SaveChangesAsync();
        var item = mapper.Map<CartItemItemModel>(entity);
        await redisService.SetAsync($"cartitem:{item.Id}",  item, TimeSpan.FromMinutes(10));
        await redisService.RemoveAsync("cartitems:all");
    }

    public async Task UpdateCartItem(Guid id, CartItemAddUpdateModel model)
    {
        var entity = await context.CartItems.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted );
        
        if (entity == null)
        {
            throw new Exception("Item not found");
        }
        mapper.Map(model, entity);
        await context.SaveChangesAsync();
        var item = mapper.Map<CartItemItemModel>(entity);
        await redisService.SetAsync($"cartitem:{item.Id}",  item, TimeSpan.FromMinutes(10));
        await redisService.RemoveAsync("cartitems:all");
        
    }

    public async Task RemoveCartItem(Guid id)
    {
        var entity = await context.CartItems.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted);
        if (entity == null)
        {
            throw new Exception("Item not found");
            
        }
        entity.IsDeleted = true;
        await context.SaveChangesAsync();
        await redisService.RemoveAsync($"cartitem:{entity.Id}");
        await redisService.RemoveAsync("cartitems:all");
    }

    public async Task RemoveAllCartItems()
    {
        var entities =  await context.CartItems.Where(x=>!x.IsDeleted).ToListAsync();
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            await redisService.RemoveAsync($"cartitem:{entity.Id}");
        }
        await redisService.RemoveAsync("cartitems:all");
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CartItemItemModel>> GetAllCartItems()
    {
        var cache = await redisService.GetAsync<List<CartItemItemModel>>($"cartitems:all");
        if (cache != null)
        {
            return cache;
        }
        var items = await context.CartItems.Where(x=>!x.IsDeleted)
            .ProjectTo<CartItemItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        await redisService.SetAsync($"cartitems:all", items, TimeSpan.FromMinutes(10));
        return items;
    }

    public async Task<CartItemItemModel> GetCartItemById(Guid id)
    {
        var cache = await redisService.GetAsync<CartItemItemModel>($"cartitem:{id}");
        if (cache != null)
        {
            return cache;
        }
        
        var entity = await context.CartItems.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted);
        if (entity == null)
        {
            throw new Exception("Item not found");
            
        }
        var item = mapper.Map<CartItemItemModel>(entity);
        await redisService.SetAsync($"cartitem:{id}", item, TimeSpan.FromMinutes(10));
        await redisService.RemoveAsync("cartitems:all");
        return item;
    }
    
}