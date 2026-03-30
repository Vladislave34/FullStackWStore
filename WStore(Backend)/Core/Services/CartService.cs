using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Cart;
using Core.Models.CartItem;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class CartService(AppStoreContext  context, IRedisService redisService, IMapper mapper, IAuthService authService) : ICartService
{
    public async Task AddCart()
    {
        var entity = new CartEntity()
        {
            UserId = await authService.GetUserIdAsync(),
            IsDeleted = false,
        };
        
        await context.Carts.AddAsync(entity);
        await context.SaveChangesAsync();
        
       
    }

    public async Task UpdateCart(Guid id, CartAddUpdateModel model)
    {
        var entity = await context.Carts.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted );
        
        if (entity == null)
        {
            throw new Exception("Item not found");
        }
        mapper.Map(model, entity);
        await context.SaveChangesAsync();
        var item = mapper.Map<CartItemModel>(entity);
       
        
    }

    public async Task RemoveCart(Guid id)
    {
        var entity = await context.Carts.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted);
        if (entity == null)
        {
            throw new Exception("Item not found");
            
        }
        entity.IsDeleted = true;
        await context.SaveChangesAsync();
        
    }

    public async Task RemoveAllCarts()
    {
        var entities =  await context.Carts.Where(x=>!x.IsDeleted).ToListAsync();
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            
        }
        
    }

    public async Task<IEnumerable<CartItemModel>> GetAllCarts()
    {
        var cache = await redisService.GetAsync<List<CartItemModel>>($"carts:all");
        if (cache != null)
        {
            return cache;
        }
        var items = await context.Carts.Where(x=>!x.IsDeleted)
            .ProjectTo<CartItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        
        return items;
    }

    public async Task<CartItemModel> GetCartById(Guid id)
    {
        var cache = await redisService.GetAsync<CartItemModel>($"cart:{id}");
        if (cache != null)
        {
            return cache;
        }
        
        var entity = await context.Carts.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted);
        if (entity == null)
        {
            throw new Exception("Item not found");
            
        }
        var item = mapper.Map<CartItemModel>(entity);
        
        return item;
    }
    
}