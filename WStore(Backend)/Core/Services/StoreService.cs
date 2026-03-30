using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Store;
using Core.Models.StoreImage;
using Domain;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class StoreService(AppStoreContext context,
    IMapper mapper, IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    UserManager<UserEntity> userManager, 
    
    IEntityImageService<StoreImageEntity, StoreImageAddUpdateModel, StoreImageItemModel> storeImageService
    )  : IStoreService
{
    public async Task AddStore(StoreAddUpdateModel model)
    {
        
        var entity = mapper.Map<StoreEntity>(model);

        
        var email = httpContextAccessor.HttpContext?.User.Claims.First().Value;
        var user = await userManager.FindByEmailAsync(email);
        entity.OwnerId = user.Id;

        
        await context.Stores.AddAsync(entity);
        await context.SaveChangesAsync();

        
        if (model.Images.Any())
        {
            await storeImageService.AddAllImages(entity.Id, model.Images.Select(f => new StoreImageAddUpdateModel { file = f }).ToList());
        }

        
        var store = mapper.Map<StoreItemModel>(entity);
        await redisService.SetAsync($"store:{store.Id}", store, TimeSpan.FromMinutes(10));
        await redisService.RemoveAsync("stores:all");

        
    }

    public async Task UpdateStore(Guid id, StoreAddUpdateModel model)
    {
        var entity = await context.Stores.FindAsync(id);

        if (entity == null)
            throw new Exception("Store not found");

        mapper.Map(model, entity);
        entity.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        var images = model.Images.Select(f => new StoreImageAddUpdateModel { file = f }).ToList(); 
        await storeImageService.UpdateAllImages(id, images);

        await context.SaveChangesAsync();

        var store = mapper.Map<StoreItemModel>(entity);

        await redisService.SetAsync($"store:{id}", store, TimeSpan.FromMinutes(10));
        await redisService.RemoveAsync("stores:all");
    }

    public async Task RemoveStore(Guid id)
    {
        var entity = await context.Stores
            .Include(s => s.Images) 
            .FirstOrDefaultAsync(s => s.Id == id);
        if (entity == null)
        {
            throw new Exception("Store not found");
        }
        entity.IsDeleted = true;
        foreach (var image in entity.Images)
        {
            image.IsDeleted = true;
            await storeImageService.DeleteImage(image.Id);
        }
        await context.SaveChangesAsync();
        await redisService.RemoveAsync($"store:{id}");
        await redisService.RemoveAsync("stores:all");
    }

    public async Task RemoveAllStores()
    {
        var entities = await context.Stores
            .Include(s => s.Images)
            .ToListAsync();
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            await storeImageService.DeleteAllImages(entity.Id);
            await redisService.RemoveAsync($"store:{entity.Id}");
        }
        await context.SaveChangesAsync();
        await redisService.RemoveAsync($"stores:all");
    }

    public async Task<IEnumerable<StoreItemModel>> GetAllStores()
    {
        string key = "stores:all";
        var cache = await redisService.GetAsync<List<StoreItemModel>>(key);
        if (cache != null)
        {
            return cache;
        }

        var stores = await context.Stores
            .Where(x => !x.IsDeleted)
            .ProjectTo<StoreItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        await redisService.SetAsync(key, stores, TimeSpan.FromMinutes(10));
        return stores;
    }

    public async Task<StoreItemModel> GetStoreById(Guid id)
    {
        string key = $"store:{id}";
        var cache = await redisService.GetAsync<StoreItemModel>(key);
        if (cache != null)
        {
            return cache;
        }

        var entity = await context.Stores
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null)
            throw new Exception("Product not found");
        var store = mapper.Map<StoreItemModel>(entity);
        await redisService.SetAsync(key, store, TimeSpan.FromMinutes(10));
        return store;
    }
}