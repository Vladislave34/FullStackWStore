using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Store;
using Core.Models.StoreImage;
using Core.Models.User;
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
    IJwtTokenService jwtTokenService,
    IAuthService authService,
    IEntityImageService<StoreImageEntity, StoreImageAddUpdateModel, StoreImageItemModel> storeImageService
    )  : IStoreService
{
    public async Task<AuthResponseModel> AddStore(StoreAddUpdateModel model)
    {
        
        var entity = mapper.Map<StoreEntity>(model);

        
        var email = httpContextAccessor.HttpContext?.User.Claims.First().Value;
        var user = await userManager.FindByEmailAsync(email);
        entity.OwnerId = user.Id;
        await userManager.AddToRoleAsync(user, "StoreOwner");

        
        await context.Stores.AddAsync(entity);
        await context.SaveChangesAsync();
        

        
        if (model.Images.Any())
        {
            await storeImageService.AddAllImages(entity.Id, model.Images.Select(f => new StoreImageAddUpdateModel { file = f }).ToList());
        }

        
        var store = mapper.Map<StoreItemModel>(entity);
        await redisService.SetAsync($"store:{store.Id}", store, TimeSpan.FromMinutes(10));
        await redisService.RemoveAsync("stores:all");

        var id = await authService.GetUserIdAsync();
        user = await userManager.FindByIdAsync(id.ToString());
        return await jwtTokenService.CreateAuthResponse(user);

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
        var store = await context.Stores
            .Include(s => s.Images)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (store == null)
        {
            throw new Exception("Store not found");
        }

        store.IsDeleted = true;

        foreach (var image in store.Images)
        {
            image.IsDeleted = true;
        }

        // Ймовірно має бути store.UserId
        var user = await userManager.FindByIdAsync(store.Id.ToString());

        if (user != null)
        {
            await userManager.RemoveFromRoleAsync(user, "StoreOwner");
        }

        var imageDeleteTasks = store.Images
            .Select(image => storeImageService.DeleteImage(image.Id));

        await Task.WhenAll(imageDeleteTasks);

        await context.SaveChangesAsync();

        await Task.WhenAll(
            redisService.RemoveAsync($"store:{id}"),
            redisService.RemoveAsync("stores:all")
        );
    }

    public async Task RemoveAllStores()
    {
        var stores = await context.Stores
            .Include(s => s.Images)
            .ToListAsync();

        foreach (var store in stores)
        {
            store.IsDeleted = true;

            var user = await userManager.FindByIdAsync(store.OwnerId.ToString());

            if (user != null)
            {
                await userManager.RemoveFromRoleAsync(user, "StoreOwner");
            }

            await Task.WhenAll(
                storeImageService.DeleteAllImages(store.Id),
                redisService.RemoveAsync($"store:{store.Id}")
            );
        }

        await context.SaveChangesAsync();
        await redisService.RemoveAsync("stores:all");
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
            .Include(s => s.Images) 
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
                .Include(s => s.Images)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted) ;
        if (entity == null)
            throw new Exception("Product not found");
        var store = mapper.Map<StoreItemModel>(entity);
        await redisService.SetAsync(key, store, TimeSpan.FromMinutes(10));
        return store;
    }
    public async Task<StoreItemModel> GetStoreByUserId()
    {
        
        var userId = await authService.GetUserIdAsync();
       

        var entity = await context.Stores
            .Include(s => s.Images)
            .FirstOrDefaultAsync(x => x.OwnerId == userId && !x.IsDeleted) ;
        if (entity == null)
            throw new Exception("Product not found");
        var store = mapper.Map<StoreItemModel>(entity);
        string key = $"store:{entity.Id}";
        await redisService.SetAsync(key, store, TimeSpan.FromMinutes(10));
        return store;
    }
}