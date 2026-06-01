using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Product;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class ProductService(AppStoreContext context, IRedisService redisService, IMapper mapper, ISearchService searchService) : IProductService
{
    public async Task AddProduct(ProductAddUpdateModel model)
    {
        var entity = mapper.Map<ProductEntity>(model);
        await context.Products.AddAsync(entity);
        await context.SaveChangesAsync();

        var fullEntity = await context.Products
            .Include(x => x.CategoryEntity)
            .Include(x => x.Store)
            .Include(x => x.Variants).ThenInclude(v => v.Color)
            .Include(x => x.Variants).ThenInclude(v => v.Size)
            .FirstOrDefaultAsync(x => x.Id == entity.Id);

        await searchService.IndexProductAsync(fullEntity!);

        var product = mapper.Map<ProductItemModel>(fullEntity!);
        await redisService.SetAsync($"product:{product.Id}:en", product, TimeSpan.FromMinutes(10));
        await redisService.SetAsync($"product:{product.Id}:uk", Localize(product, "uk"), TimeSpan.FromMinutes(10));
        await redisService.RemoveAsync("products:all:en");
        await redisService.RemoveAsync("products:all:uk");
    }

    public async Task UpdateProduct(Guid id, ProductAddUpdateModel model)
    {
        var entity = await context.Products.FindAsync(id)
            ?? throw new Exception("Product not found");

        mapper.Map(model, entity);
        entity.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        await context.SaveChangesAsync();

        var fullEntity = await context.Products
            .Include(x => x.CategoryEntity)
            .Include(x => x.Store)
            .Include(x => x.Variants).ThenInclude(v => v.Color)
            .Include(x => x.Variants).ThenInclude(v => v.Size)
            .FirstOrDefaultAsync(x => x.Id == id);

        await searchService.IndexProductAsync(fullEntity!);

        var product = mapper.Map<ProductItemModel>(fullEntity!);
        await redisService.SetAsync($"product:{id}:en", product, TimeSpan.FromMinutes(10));
        await redisService.SetAsync($"product:{id}:uk", Localize(product, "uk"), TimeSpan.FromMinutes(10));
        await redisService.RemoveAsync("products:all:en");
        await redisService.RemoveAsync("products:all:uk");
    }

    public async Task RemoveProduct(Guid id)
    {
        var entity = await context.Products.FindAsync(id)
            ?? throw new Exception("Product not found");

        entity.IsDeleted = true;
        await context.SaveChangesAsync();
        await searchService.DeleteProductAsync(id);
        await redisService.RemoveAsync($"product:{id}:en");
        await redisService.RemoveAsync($"product:{id}:uk");
        await redisService.RemoveAsync("products:all:en");
        await redisService.RemoveAsync("products:all:uk");
    }

    public async Task RemoveAllProducts()
    {
        var entities = await context.Products.ToListAsync();
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            await redisService.RemoveAsync($"product:{entity.Id}:en");
            await redisService.RemoveAsync($"product:{entity.Id}:uk");
            await searchService.DeleteProductAsync(entity.Id);
        }
        await context.SaveChangesAsync();
        await redisService.RemoveAsync("products:all:en");
        await redisService.RemoveAsync("products:all:uk");
    }

    public async Task<IEnumerable<ProductItemModel>> GetAllProducts(string lang)
    {
        string key = $"products:all:{lang}";
        var cache = await redisService.GetAsync<List<ProductItemModel>>(key);
        if (cache != null) return cache;

        var products = await context.Products
            .Where(x => !x.IsDeleted)
            .ProjectTo<ProductItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        var localized = products.Select(p => Localize(p, lang)).ToList();
        await redisService.SetAsync(key, localized, TimeSpan.FromMinutes(10));
        return localized;
    }

    public async Task<ProductItemModel> GetProductById(Guid id, string lang)
    {
        string key = $"product:{id}:{lang}";
        var cache = await redisService.GetAsync<ProductItemModel>(key);
        if (cache != null) return cache;

        var entity = await context.Products
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new Exception("Product not found");

        var product = Localize(mapper.Map<ProductItemModel>(entity), lang);
        await redisService.SetAsync(key, product, TimeSpan.FromMinutes(10));
        return product;
    }

    private static ProductItemModel Localize(ProductItemModel model, string lang)
    {
        if (lang.StartsWith("uk", StringComparison.OrdinalIgnoreCase))
        {
            model.Name = model.NameUk ?? model.Name;
            model.Description = model.DescriptionUk ?? model.Description;
        }
        return model;
    }
}