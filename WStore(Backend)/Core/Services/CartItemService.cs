using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.CartItem;
using Core.Models.Product;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class CartItemService(AppStoreContext  context, IRedisService redisService, IMapper mapper, IAuthService authService, ISearchService searchService) : ICartItemService
{
    public async Task AddCartItem(CartItemAddUpdateModel model)
    {
        var existing = await context.CartItems
            .FirstOrDefaultAsync(x => x.CartId == model.CartId
                                      && x.ProductVariantId == model.ProductVariantId
                                      && !x.IsDeleted);

        if (existing != null)
        {
            existing.Quantity += model.Quantity;
            existing.Price += model.Price * model.Quantity;
            await context.SaveChangesAsync();
            var item_ = mapper.Map<CartItemItemModel>(existing);
            await redisService.SetAsync($"cartitem:{item_.Id}", item_, TimeSpan.FromMinutes(10));
            await redisService.RemoveAsync("cartitems:all");
            await redisService.RemoveByPrefixAsync("cartitems:all:");
            //await searchService.IndexCartItemAsync(existing);
            return;
        }

        var entity = mapper.Map<CartItemEntity>(model);
        await context.CartItems.AddAsync(entity);
        await context.SaveChangesAsync();
        //await searchService.IndexCartItemAsync(entity);

        var item = mapper.Map<CartItemItemModel>(entity);
        await redisService.SetAsync($"cartitem:{item.Id}", item, TimeSpan.FromMinutes(10));
        await redisService.RemoveAsync("cartitems:all");
        await redisService.RemoveByPrefixAsync("cartitems:all:");
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
        await redisService.RemoveByPrefixAsync("cartitems:all:");
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
        await redisService.RemoveByPrefixAsync("cartitems:all:");
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
        await redisService.RemoveByPrefixAsync("cartitems:all:");
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
        await redisService.RemoveByPrefixAsync("cartitems:all:");
        return item;
    }

    public async Task<IEnumerable<CartItemItemModel>> GetCartItemsByUser()
    {
        
        var userId = await authService.GetUserIdAsync();
        string cacheKey = $"cartitems:all:{userId}";
        var cache = await redisService.GetAsync<List<CartItemItemModel>>(cacheKey);
        if (cache != null) return cache;
        var entities = await context.CartItems
            .Where(x=>x.Cart.UserId == userId && !x.IsDeleted)
            .Include(x=>x.ProductVariant)
            .Include(x=>x.ProductVariant.Color)
            .Include(x=>x.ProductVariant.Size)
            .Include(x=>x.ProductVariant.Sale)
            .Include(x=>x.ProductVariant.Image.Where(i=>!i.IsDeleted))
            .Include(x=>x.ProductVariant.Product)
            .ToListAsync();
        var items = mapper.Map<List<CartItemItemModel>>(entities);
        await redisService.SetAsync(cacheKey, items, TimeSpan.FromMinutes(10));
        return items;
    }

    public async Task<PageResult<CartItemItemModel>> SearchCartItems(
        string query, string lang, int pageNumber = 1, int pageSize = 10)
    {
        var userId = await authService.GetUserIdAsync();
        string key = $"cartitems:search:query:{query}:{lang}:{pageNumber}:{pageSize}";
        var cached = await redisService.GetAsync<PageResult<CartItemItemModel>>(key);

        if (cached != null)
            return cached;

        var (ids, totalCount) = await searchService.SearchCartItemAsync(query, lang, pageNumber, pageSize);

        var entities = await context.CartItems
            .Include(x => x.ProductVariant)
            .Include(x => x.ProductVariant.Color)
            .Include(x => x.ProductVariant.Size)
            .Include(x => x.ProductVariant.Sale)
            .Include(x => x.ProductVariant.Image.Where(i=>!i.IsDeleted))
            .Include(x => x.ProductVariant.Product)
            .Where(x => ids.Contains(x.Id) && !x.IsDeleted && x.Cart.UserId == userId)
            .AsNoTracking()
            .ToListAsync();

        var mapped = mapper.Map<List<CartItemItemModel>>(entities);

        var ordered = ids
            .Select(id => mapped.FirstOrDefault(e => e.Id == id))
            .Where(e => e != null)
            .Select(e => e!)
            .ToList();

        var result = new PageResult<CartItemItemModel>
        {
            Data = ordered,
            TotalCount = totalCount,
            CurrentPage = pageNumber,
            PageSize = pageSize
        };

        await redisService.SetAsync(key, result, TimeSpan.FromMinutes(10));
        return result;
    }
    
}