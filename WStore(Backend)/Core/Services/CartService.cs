using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Cart;
using Core.Models.CartItem;
using Domain;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class CartService(AppStoreContext  context, IRedisService redisService, IMapper mapper, IAuthService authService, UserManager<UserEntity> userManager) : ICartService
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    private static string CartByIdKey(Guid id) => $"cart:{id}";
    private static string CartByUserKey(Guid userId) => $"cart:user:{userId}";
    private const string AllCartsKey = "carts:all";

    public async Task<Guid> AddCart()
    {
        Guid userId = await authService.GetUserIdAsync();

        if (await context.Carts.AnyAsync(x => x.UserId == userId && !x.IsDeleted))
            throw new Exception("User had cart");

        var entity = new CartEntity()
        {
            UserId = userId,
            IsDeleted = false,
        };

        await context.Carts.AddAsync(entity);
        await context.SaveChangesAsync();

        await redisService.RemoveAsync(AllCartsKey);
        await redisService.RemoveAsync(CartByUserKey(userId));
        return entity.Id;
    }

    public async Task<CartItemModel> GetCartItemByUser()
    {
        Guid userId = await authService.GetUserIdAsync();
        var cacheKey = CartByUserKey(userId);

        var cache = await redisService.GetAsync<CartItemModel>(cacheKey);
        if (cache != null) return cache;

        var entity = await context.Carts.Include(x=>x.Items).FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);
        if (entity == null)
            throw new Exception("Cart not found");

        var item = mapper.Map<CartItemModel>(entity);
        await redisService.SetAsync(cacheKey, item, CacheDuration);

        return item;
    }

    public async Task UpdateCart(Guid id, CartAddUpdateModel model)
    {
        var entity = await context.Carts.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (entity == null)
        {
            throw new Exception("Item not found");
        }

        mapper.Map(model, entity);
        await context.SaveChangesAsync();

        var item = mapper.Map<CartItemModel>(entity);

        await redisService.SetAsync(CartByIdKey(id), item, CacheDuration);
        await redisService.RemoveAsync(AllCartsKey);
        await redisService.RemoveAsync(CartByUserKey(entity.UserId));
    }

    public async Task RemoveCart(Guid id)
    {
        var entity = await context.Carts.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null)
        {
            throw new Exception("Item not found");
        }

        entity.IsDeleted = true;
        await context.SaveChangesAsync();

        await redisService.RemoveAsync(CartByIdKey(id));
        await redisService.RemoveAsync(AllCartsKey);
        await redisService.RemoveAsync(CartByUserKey(entity.UserId));
    }

    public async Task RemoveAllCarts()
    {
        var entities = await context.Carts.Where(x => !x.IsDeleted).ToListAsync();
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
        }

        await context.SaveChangesAsync(); 

        await redisService.RemoveAsync(AllCartsKey);
        
        await redisService.RemoveByPrefixAsync("cart:");
    }

    public async Task<IEnumerable<CartItemModel>> GetAllCarts()
    {
        var cache = await redisService.GetAsync<List<CartItemModel>>(AllCartsKey);
        if (cache != null)
        {
            return cache;
        }

        var items = await context.Carts.Where(x => !x.IsDeleted)
            .ProjectTo<CartItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        await redisService.SetAsync(AllCartsKey, items, CacheDuration);

        return items;
    }

    public async Task<CartItemModel> GetCartById(Guid id)
    {
        var cacheKey = CartByIdKey(id);
        var cache = await redisService.GetAsync<CartItemModel>(cacheKey);
        if (cache != null)
        {
            return cache;
        }

        var entity = await context.Carts.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null)
        {
            throw new Exception("Item not found");
        }

        var item = mapper.Map<CartItemModel>(entity);
        await redisService.SetAsync(cacheKey, item, CacheDuration);

        return item;
    }

    public async Task<bool> HasCart()
    {
        Guid userId = await authService.GetUserIdAsync();
        var cacheKey = CartByUserKey(userId);

        
        var cache = await redisService.GetAsync<CartItemModel>(cacheKey);
        if (cache != null) return true;

        return await context.Carts.AnyAsync(c => c.UserId == userId && !c.IsDeleted);
    }
}