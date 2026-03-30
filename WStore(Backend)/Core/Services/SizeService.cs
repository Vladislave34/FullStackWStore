using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Color;
using Core.Models.Size;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class SizeService(AppStoreContext context, IMapper mapper, IRedisService redisService)  : ISizeService
{
    public async Task AddSize(SizeAddUpdateModel model)
    {
        var entity = mapper.Map<SizeEntity>(model);

        await context.Sizes.AddAsync(entity);
        await context.SaveChangesAsync();

        
        var item = mapper.Map<SizeItemModel>(entity);
        await redisService.SetAsync($"size:{entity.Id}", item, TimeSpan.FromMinutes(30));

        
        await redisService.RemoveAsync("sizes:all");
    }

    public async Task UpdateSize(Guid id, SizeAddUpdateModel model)
    {
        var entity = await context.Sizes.FindAsync(id);
        if (entity == null)
            return;

        mapper.Map(model, entity);
        entity.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        await context.SaveChangesAsync();
        
        var item = mapper.Map<SizeItemModel>(entity);
        await redisService.SetAsync($"size:{id}", item, TimeSpan.FromMinutes(30));

        await redisService.RemoveAsync("sizes:all");
        
    }

    public async Task RemoveSize(Guid id)
    {
        var old_entity = await context.Sizes.FindAsync(id);
        old_entity.IsDeleted = true;
        await context.SaveChangesAsync();
        await redisService.RemoveAsync($"size:{id}");
        await redisService.RemoveAsync("sizes:all");
    }

    public async Task RemoveAllSizes()
    {
        foreach (var entity in await context.Sizes.ToListAsync())
        {
            entity.IsDeleted = true;
            await redisService.RemoveAsync($"size:{entity.Id}");
        }
        await context.SaveChangesAsync();
        await redisService.RemoveAsync("sizes:all");
    }

    public async Task<IEnumerable<SizeItemModel>> GetAllSizes()
    {
        string key = "sizes:all";

        var cached = await redisService.GetAsync<IEnumerable<SizeItemModel>>(key);
        if (cached != null)
            return cached;

        var sizes = await context.Sizes
            .Where(x => !x.IsDeleted)
            .ProjectTo<SizeItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        await redisService.SetAsync(key, sizes, TimeSpan.FromMinutes(10));

        return sizes;
    }

    public async Task<SizeItemModel> GetSizeById(Guid id)
    {
        string key = $"size:{id}";
        var cached = await redisService.GetAsync<SizeItemModel>(key);
        if (cached != null)
        {
            return cached;
        }

        var entity = await context.Sizes.FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);
        if (entity == null)
            throw new Exception("Product not found");

        
        var model = mapper.Map<SizeItemModel>(entity);

        
        await redisService.SetAsync(key, model, TimeSpan.FromMinutes(30));

        return model;
        
        
    }
}