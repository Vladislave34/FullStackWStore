using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Product;
using Domain;
using Microsoft.EntityFrameworkCore;

using Domain.Entities;

namespace Core.Services;

public class ProductService(AppStoreContext context, IRedisService redisService, IMapper mapper, ISearchService searchService)  : IProductService
{
    public async Task AddProduct(ProductAddUpdateModel model)
    {
        var entity = mapper.Map<ProductEntity>(model);
        await context.Products.AddAsync(entity);
        await context.SaveChangesAsync();
        var fullEntity = await context.Products
            .Include(x => x.CategoryEntity)
            .Include(x => x.Store)
            .Include(x => x.Variants)
            .ThenInclude(v => v.Color)
            .Include(x => x.Variants)
            .ThenInclude(v => v.Size)
            .FirstOrDefaultAsync(x => x.Id == entity.Id);

        await searchService.IndexProductAsync(fullEntity!);
        var product = mapper.Map<ProductItemModel>(entity);
        await redisService.SetAsync($"product:{product.Id}", product, TimeSpan.FromMinutes(10));
        await redisService.RemoveAsync($"products:all");
    }

    public async Task UpdateProduct(Guid id, ProductAddUpdateModel model)
    {
        var entity = await context.Products.FindAsync(id);
        if (entity == null)
        {
            throw new Exception("Product not found");
        }
        mapper.Map(model, entity);
        entity.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        await context.SaveChangesAsync();
        var fullEntity = await context.Products
            .Include(x => x.CategoryEntity)
            .Include(x => x.Store)
            .Include(x => x.Variants)
            .ThenInclude(v => v.Color)
            .Include(x => x.Variants)
            .ThenInclude(v => v.Size)
            .FirstOrDefaultAsync(x => x.Id == id);

        await searchService.IndexProductAsync(fullEntity!);
        var product = mapper.Map<ProductItemModel>(entity);
        await redisService.SetAsync($"product:{product.Id}", product, TimeSpan.FromMinutes(10));
        await redisService.RemoveAsync($"products:all");
    }

    public async Task RemoveProduct(Guid id)
    {
        var entity = await context.Products.FindAsync(id);
        if (entity == null)
        {
            throw new Exception("Product not found");
        }
        entity.IsDeleted = true;
        await context.SaveChangesAsync();
        await searchService.DeleteProductAsync(id);
        await redisService.RemoveAsync($"product:{id}");
        await redisService.RemoveAsync($"products:all");
    }

    public async Task RemoveAllProducts()
    {
        var entities = await context.Products.ToListAsync();
        foreach (var entity in entities)
        {
            
            entity.IsDeleted = true;
            
            await redisService.RemoveAsync($"product:{entity.Id}");
            await searchService.DeleteProductAsync(entity.Id);
        }
        await context.SaveChangesAsync();
        await redisService.RemoveAsync($"products:all");
    }

    public async Task<IEnumerable<ProductItemModel>> GetAllProducts()
    {
        string key = "products:all";
        var cache = await redisService.GetAsync<List<ProductItemModel>>(key);
        if (cache != null)
        {
            return cache;
        }

        var products = await context.Products.Where(x => !x.IsDeleted)
            .ProjectTo<ProductItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        await redisService.SetAsync(key, products, TimeSpan.FromMinutes(10));
        return products;
    }

    public async Task<ProductItemModel> GetProductById(Guid id)
    {
        string key = $"product:{id}";
        var cache = await redisService.GetAsync<ProductItemModel>(key);
        if (cache != null)
        {
            return cache;
        }

        var entity =await context.Products.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        
        if (entity == null)
        {
            throw new Exception("Product not found");
        }
        var product = mapper.Map<ProductItemModel>(entity);
        await redisService.SetAsync(key, product, TimeSpan.FromMinutes(10));
        
        return product;
    }
}