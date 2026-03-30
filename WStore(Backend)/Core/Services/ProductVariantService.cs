
using AutoMapper;
using Core.Interfaces;
using Core.Models.ProductVariant;
using Core.Models.ProductVariantImage;

using Domain;
using Domain.Entities;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class ProductVariantService(
    AppStoreContext context,
    IRedisService redisService,
    IMapper mapper,
    ISearchService searchService,
    IEntityImageService<ProductVariantImageEntity,
            ProductVariantImageAddModel, 
            ProductVariantImageItemModel>
            productImageService
    ): IProductVariantServices
{
    public async Task AddProductVariant(ProductVariantAddUpdateModel model)
    {
        var entity = mapper.Map<ProductVariantEntity>(model);
        await context.ProductVariants.AddAsync(entity);
        await context.SaveChangesAsync();

        if (model.Images.Any())
        {
            await productImageService.AddAllImages(entity.Id, model.Images.Select(f => new ProductVariantImageAddModel() { file = f }).ToList());
        }
        var fullProduct = await context.Products
            .Include(x => x.CategoryEntity)
            .Include(x => x.Store)
            .Include(x => x.Variants)
            .ThenInclude(v => v.Color)
            .Include(x => x.Variants)
            .ThenInclude(v => v.Size)
            .FirstOrDefaultAsync(x => x.Id == entity.ProductId);

        if (fullProduct != null)
            await searchService.IndexProductAsync(fullProduct);
        var variant = mapper.Map<ProductVariantItemModel>(entity);
        await redisService.SetAsync($"productvariant:{variant.Id}", variant, TimeSpan.FromMinutes(10));
        await redisService.RemoveAsync($"productvariants:all");
    }

    public async Task UpdateProductVariant(Guid id, ProductVariantAddUpdateModel model)
    {
        var entity = await context.ProductVariants.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted);
        if (entity == null)
        {
            throw new Exception("Store not found");
        }
        mapper.Map(model, entity);
        await productImageService.UpdateAllImages(entity.Id,
            model.Images.Select(f => new ProductVariantImageAddModel() { file = f }).ToList());
        
        await context.SaveChangesAsync();
        
        var fullProduct = await context.Products
            .Include(x => x.CategoryEntity)
            .Include(x => x.Store)
            .Include(x => x.Variants)
            .ThenInclude(v => v.Color)
            .Include(x => x.Variants)
            .ThenInclude(v => v.Size)
            .FirstOrDefaultAsync(x => x.Id == entity.ProductId);

        if (fullProduct != null)
            await searchService.IndexProductAsync(fullProduct);
        
        var variant = mapper.Map<ProductVariantItemModel>(entity);
        await redisService.SetAsync($"productvariant:{entity.Id}", variant, TimeSpan.FromMinutes(10));
        await redisService.RemoveAsync("productvariants:all");
    }

    public async Task RemoveProductVariant(Guid id)
    {
        var entity = await context.ProductVariants.FirstOrDefaultAsync(x=>x.Id == id && !x.IsDeleted);
        
        if (entity == null)
        {
            throw new Exception("Store not found");
        }
        entity.IsDeleted = true;
       
        foreach (var img in entity.Image)
        {
            img.IsDeleted = true;
            await productImageService.DeleteImage(img.Id);
        }
        await context.SaveChangesAsync();
        await redisService.RemoveAsync($"productvariant:{id}");
        
    }

    public async Task RemoveAllProductVariants()
    {
        var entities = await context.ProductVariants.Where(x=>!x.IsDeleted).ToListAsync();
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            await productImageService.DeleteImage(entity.Id);
            await redisService.RemoveAsync($"productvariant:{entity.Id}");
        }
        await context.SaveChangesAsync();
        
    }

    public async Task<IEnumerable<ProductVariantItemModel>> GetAllProductVariantsByProductId(Guid productId)
    {
        string key = $"productvariants:product:{productId}";
        var cache = await redisService.GetAsync<List<ProductVariantItemModel>>(key);
        if (cache != null) return cache;
        var variants = await context.ProductVariants.Where(x=>!x.IsDeleted && x.ProductId == productId)
            .ProjectTo<ProductVariantItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        await redisService.SetAsync(key, variants, TimeSpan.FromMinutes(10));
        return variants;
        
    }

    public async Task<ProductVariantItemModel> GetProductVariantById(Guid id)
    {
        string key = $"productvariant:{id}";
        var cache = await redisService.GetAsync<ProductVariantItemModel>(key);
        if (cache != null)
        {
            return cache;
        }
        var entity = await context.ProductVariants
            .Include(x => x.Image)
            .FirstOrDefaultAsync(x=> x.Id == id && !x.IsDeleted);
        var variant = mapper.Map<ProductVariantItemModel>(entity);
        await redisService.SetAsync(key, variant, TimeSpan.FromMinutes(10));
        return variant;


    }
}