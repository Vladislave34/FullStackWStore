using AutoMapper;
using Core.Interfaces;
using Core.Models.ProductVariant;
using Core.Models.ProductVariantImage;
using Domain;
using Domain.Entities;
using AutoMapper.QueryableExtensions;
using Core.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class ProductVariantService(
    AppStoreContext context,
    IRedisService redisService,
    IMapper mapper,
    IEntityImageService<ProductVariantImageEntity,
            ProductVariantImageAddModel,
            ProductVariantImageItemModel>
            productImageService
    ) : IProductVariantServices
{
    public async Task AddProductVariant(ProductVariantAddUpdateModel model)
    {
        var entity = mapper.Map<ProductVariantEntity>(model);
        await context.ProductVariants.AddAsync(entity);
        await context.SaveChangesAsync();

        if (model.Images.Any())
        {
            await productImageService.AddAllImages(entity.Id,
                model.Images.Select(f => new ProductVariantImageAddModel() { file = f }).ToList());
        }

        // ключова зміна: перезавантажуємо entity з актуальними зображеннями
        var reloaded = await context.ProductVariants
            .Include(x => x.Image)
            .AsNoTracking()
            .FirstAsync(x => x.Id == entity.Id);

        var variant = mapper.Map<ProductVariantItemModel>(reloaded);
        await redisService.SetAsync($"productvariant:{variant.Id}", variant, TimeSpan.FromMinutes(10));
        await redisService.RemoveByPrefixAsync($"productvariants:{entity.ProductId}");
        await redisService.RemoveByPrefixAsync($"products:");
        await redisService.RemoveByPrefixAsync($"product:");
    }

    public async Task UpdateProductVariant(Guid id, ProductVariantAddUpdateModel model)
    {
        var entity = await context.ProductVariants
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (entity == null)
            throw new Exception("ProductVariant not found");

        mapper.Map(model, entity);
        await productImageService.UpdateAllImages(entity.Id,
            model.Images.Select(f => new ProductVariantImageAddModel() { file = f }).ToList());

        await context.SaveChangesAsync();

        // перезавантажуємо з актуальними зображеннями
        var reloaded = await context.ProductVariants
            .Include(x => x.Image)
            .AsNoTracking()
            .FirstAsync(x => x.Id == entity.Id);

        var variant = mapper.Map<ProductVariantItemModel>(reloaded);
        await redisService.SetAsync($"productvariant:{entity.Id}", variant, TimeSpan.FromMinutes(10));
        await redisService.RemoveByPrefixAsync($"productvariants:{entity.ProductId}");
        await redisService.RemoveByPrefixAsync($"products:");
        await redisService.RemoveByPrefixAsync($"product:");
    }

    public async Task RemoveProductVariant(Guid id)
    {
        var entity = await context.ProductVariants
            .Include(x => x.Image)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (entity == null)
            throw new Exception("ProductVariant not found");

        entity.IsDeleted = true;

        foreach (var img in entity.Image)
        {
            img.IsDeleted = true;
            await productImageService.DeleteImage(img.Id);
        }

        await context.SaveChangesAsync();
        await redisService.RemoveAsync($"productvariant:{id}");
        await redisService.RemoveByPrefixAsync($"productvariants:{entity.ProductId}");
    }

    public async Task RemoveAllProductVariants()
    {
        var entities = await context.ProductVariants
            .Where(x => !x.IsDeleted)
            .ToListAsync();

        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            await productImageService.DeleteImage(entity.Id);
            await redisService.RemoveAsync($"productvariant:{entity.Id}");
        }

        await context.SaveChangesAsync();
        await redisService.RemoveByPrefixAsync("productvariants:");
    }

    public async Task<IEnumerable<ProductVariantItemModel>> GetAllProductVariantsByProductId(
        Guid productId)
    {
        string key = $"productvariants:{productId}";
        var cached = await redisService.GetAsync<IEnumerable<ProductVariantItemModel>>(key);
        if (cached != null) return cached;

        var query = context.ProductVariants
            .Where(x => !x.IsDeleted && x.ProductId == productId);

       
        var productVariants = await query
            .OrderBy(x => x.CreatedAt)
            .Include(x => x.Image)
            .ProjectTo<ProductVariantItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        

        await redisService.SetAsync(key, productVariants, TimeSpan.FromMinutes(10));
        return productVariants;
    }

    public async Task<ProductVariantItemModel> GetProductVariantById(Guid id)
    {
        string key = $"productvariant:{id}";
        var cache = await redisService.GetAsync<ProductVariantItemModel>(key);
        if (cache != null) return cache;

        var entity = await context.ProductVariants
            .Include(x => x.Image)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        var variant = mapper.Map<ProductVariantItemModel>(entity);
        await redisService.SetAsync(key, variant, TimeSpan.FromMinutes(10));
        return variant;
    }
}