using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Category;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class CategoryService(AppStoreContext context, IMapper mapper, IMinioImageService imageService, IRedisService redisService) : ICategoryService
{
    public async Task AddCategory(CategoryAddUpdateModel model)
    {
        var entity = mapper.Map<CategoryEntity>(model);
        var image = await imageService.UploadImageAsync(model.Image);
        entity.image = image;
        await context.Categories.AddAsync(entity);
        await context.SaveChangesAsync();
        var item = mapper.Map<CategoryItemModel>(entity);
        await redisService.SetAsync($"category:{entity.Id}", item, TimeSpan.FromMinutes(30));

        
        await redisService.RemoveAsync("categories:all");
    }

    public async Task UpdateCategory(Guid id, CategoryAddUpdateModel model)
    {
        var entity = await context.Categories.FindAsync(id);
        
        mapper.Map(model, entity);
        entity.image = await imageService.UpdateImageAsync(entity.image,  model.Image);
        entity.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        await context.SaveChangesAsync();
        var item = mapper.Map<CategoryItemModel>(entity);
        await redisService.SetAsync($"category:{entity.Id}", item, TimeSpan.FromMinutes(30));
        await redisService.RemoveAsync("categories:all");
    }

    public async Task RemoveCategory(Guid id)
    {
        var entity = await context.Categories.FindAsync(id);
        entity.IsDeleted = true;
        await imageService.DeleteImageAsync(entity.image);
        await context.SaveChangesAsync();
        await redisService.RemoveAsync($"category:{id}");
        await redisService.RemoveAsync("categories:all");
        
    }

    public async Task RemoveAllCategories()
    {
        var entities = await context.Categories.ToListAsync();
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            await imageService.DeleteImageAsync(entity.image);
            await redisService.RemoveAsync($"category:{entity.Id}");
        }
        await context.SaveChangesAsync();
        await redisService.RemoveAsync("categories:all");
    }

    public async Task<IEnumerable<CategoryItemModel>> GetAllCategories()
    {
        string key = "categories:all";
        
        var cache = await redisService.GetAsync<List<CategoryItemModel>>(key);
        if (cache != null)
        {
            return cache;
        }
        var categories = await context.Categories
            .Where(x=>!x.IsDeleted)
            .ProjectTo<CategoryItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        await redisService.SetAsync(key, categories, TimeSpan.FromMinutes(10));

        return categories;
    }

    public async Task<CategoryItemModel> GetCategoryById(Guid id)
    {
        string key = $"category:{id}";
        
        var cache = await redisService.GetAsync<CategoryItemModel>(key);
        if (cache != null)
        {
            return cache;
        }
        var entity  = await context.Categories.SingleOrDefaultAsync(x=> x.Id == id && !x.IsDeleted);
        if (entity == null)
            throw new Exception("Product not found");
        var category = mapper.Map<CategoryItemModel>(entity);
        await redisService.SetAsync(key, category, TimeSpan.FromMinutes(10));
        return category;
    }
}