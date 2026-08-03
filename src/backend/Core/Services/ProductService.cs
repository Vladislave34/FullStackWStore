using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Product;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


public class ProductService(
    AppStoreContext context,
    IRedisService redisService,
    IMapper mapper,
    ISearchService searchService,
    IAuthService authService) : IProductService
{
    private IQueryable<ProductEntity> ProductWithIncludes() =>
        context.Products
            .Include(x => x.CategoryEntity)
            .Include(x => x.Store)
            .Include(x => x.GenderEntity)
            .Include(x => x.Variants).ThenInclude(v => v.Color)
            .Include(x => x.Variants).ThenInclude(v => v.Size)
            .Include(x => x.Variants).ThenInclude(v => v.Sale)
            .Include(x => x.Variants).ThenInclude(v => v.Image.Where(i => !i.IsDeleted));

    
    private async Task<HashSet<Guid>?> GetFavouriteIdsAsync()
    {
        Guid userId;
        try
        {
            userId = await authService.GetUserIdAsync();
        }
        catch
        {
            return null;
        }

        if (userId == Guid.Empty) return null;

        var ids = await context.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Favourites)
            .Select(p => p.Id)
            .ToListAsync();

        return ids.ToHashSet();
    }

    private static void ApplyFavourites(List<ProductItemModel> products, HashSet<Guid>? favouriteIds)
    {
        foreach (var p in products)
        {
            p.IsFavourite = favouriteIds == null ? null : favouriteIds.Contains(p.Id);
        }
    }

    public async Task AddProduct(ProductAddUpdateModel model)
    {
        var entity = mapper.Map<ProductEntity>(model);
        await context.Products.AddAsync(entity);
        await context.SaveChangesAsync();

        var fullEntity = await ProductWithIncludes()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == entity.Id);

        await searchService.IndexProductAsync(fullEntity!);

        var productEn = mapper.Map<ProductItemModel>(fullEntity!);
        var productUk = Localize(mapper.Map<ProductItemModel>(fullEntity!), "uk");

        // IsFavourite тут завжди null/false - товар щойно створений, ще ні в кого немає в улюблених
        productEn.IsFavourite = null;
        productUk.IsFavourite = null;

        await redisService.SetAsync($"product:{productEn.Id}:en", productEn, TimeSpan.FromMinutes(10));
        await redisService.SetAsync($"product:{productEn.Id}:uk", productUk, TimeSpan.FromMinutes(10));
        await redisService.RemoveByPrefixAsync("products:all:");
        await redisService.RemoveByPrefixAsync($"products:store:{entity.StoreId}:");
    }

    public async Task UpdateProduct(Guid id, ProductAddUpdateModel model)
    {
        var userId = await authService.GetUserIdAsync();
        var entity = await context.Products.FindAsync(id)
            ?? throw new Exception("Product not found");
        if(entity.Store.OwnerId != userId) throw new Exception("This product is not owned by the user");

        mapper.Map(model, entity);
        entity.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        await context.SaveChangesAsync();

        var fullEntity = await ProductWithIncludes()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        await searchService.IndexProductAsync(fullEntity!);

        var productEn = mapper.Map<ProductItemModel>(fullEntity!);
        var productUk = Localize(mapper.Map<ProductItemModel>(fullEntity!), "uk");

        productEn.IsFavourite = null;
        productUk.IsFavourite = null;

        await redisService.SetAsync($"product:{id}:en", productEn, TimeSpan.FromMinutes(10));
        await redisService.SetAsync($"product:{id}:uk", productUk, TimeSpan.FromMinutes(10));
        await redisService.RemoveByPrefixAsync("products:all:");
        await redisService.RemoveByPrefixAsync($"products:store:{entity.StoreId}:");
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
        await redisService.RemoveByPrefixAsync("products:all:");
        await redisService.RemoveByPrefixAsync($"products:store:{entity.StoreId}:");
        await redisService.RemoveByPrefixAsync("products:category:");
    }

    public async Task RemoveAllProducts()
    {
        var entities = await context.Products.ToListAsync();
        var storeIds = entities.Select(e => e.StoreId).Distinct().ToList();

        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            await redisService.RemoveAsync($"product:{entity.Id}:en");
            await redisService.RemoveAsync($"product:{entity.Id}:uk");
            await searchService.DeleteProductAsync(entity.Id);
        }

        await context.SaveChangesAsync();
        await redisService.RemoveByPrefixAsync("products:all:");
        await redisService.RemoveByPrefixAsync("products:category:");
        foreach (var storeId in storeIds)
        {
            await redisService.RemoveByPrefixAsync($"products:store:{storeId}:");
        }
    }

    public async Task<PageResult<ProductItemModel>> GetAllProducts(string lang, int pageNumber = 1, int pageSize = 10)
    {
        string key = $"products:all:{lang}:{pageNumber}:{pageSize}";
        var cached = await redisService.GetAsync<PageResult<ProductItemModel>>(key);

        var favouriteIds = await GetFavouriteIdsAsync();

        if (cached != null)
        {
            ApplyFavourites(cached.Data, favouriteIds);
            return cached;
        }

        var query = context.Products.Where(x => !x.IsDeleted);
        var totalCount = await query.CountAsync();

        var products = await ProductWithIncludes()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        var mapped = mapper.Map<List<ProductItemModel>>(products);
        var localized = mapped.Select(p => Localize(p, lang)).ToList();

        var result = new PageResult<ProductItemModel>
        {
            Data = localized,
            TotalCount = totalCount,
            CurrentPage = pageNumber,
            PageSize = pageSize
        };

        await redisService.SetAsync(key, result, TimeSpan.FromMinutes(10));

        ApplyFavourites(result.Data, favouriteIds);
        return result;
    }

    public async Task<PageResult<ProductItemModel>> GetAllProducts(string lang, string? query,
        Guid? categoryId, Guid? genderId, Guid? colorId, Guid? sizeId,
         int pageNumber = 1, int pageSize = 10)
    {
        string key = $"products:category:{categoryId}:gender:{genderId}:color:{colorId}:size:{sizeId}:query:{query}:{lang}:{pageNumber}:{pageSize}";
        var cached = await redisService.GetAsync<PageResult<ProductItemModel>>(key);
        var favouriteIds = await GetFavouriteIdsAsync();

        if (cached != null)
        {
            ApplyFavourites(cached.Data, favouriteIds);
            return cached;
        }

        var (ids, totalCount) = await searchService.SearchAsync(query, lang, categoryId, genderId, colorId, sizeId,  pageNumber, pageSize);

        var entities = await ProductWithIncludes()
            .Where(x => ids.Contains(x.Id) && !x.IsDeleted)
            .AsNoTracking()
            .ToListAsync();

        var mapped = mapper.Map<List<ProductItemModel>>(entities);

        var ordered = ids
            .Select(id => mapped.FirstOrDefault(e => e.Id == id))
            .Where(e => e != null)
            .Select(e => e!)
            .ToList();

        var localized = ordered.Select(p => Localize(p, lang)).ToList();

        var result = new PageResult<ProductItemModel>
        {
            Data = localized,
            TotalCount = totalCount,
            CurrentPage = pageNumber,
            PageSize = pageSize
        };

        await redisService.SetAsync(key, result, TimeSpan.FromMinutes(10));
        ApplyFavourites(result.Data, favouriteIds);
        return result;
    }

    public async Task<PageResult<ProductItemModel>> GetAllProducts(string lang, string? query, Guid? categoryId,
        Guid? genderId, bool? hasSale, Guid? colorId, Guid? sizeId,
        int pageNumber = 1, int pageSize = 10)
    {
        string key = $"products:category:{categoryId}:gender:{genderId}:color:{colorId}:size:{sizeId}:hasSale:{hasSale}:query:{query}:{lang}:{pageNumber}:{pageSize}";
        var cached = await redisService.GetAsync<PageResult<ProductItemModel>>(key);
        var favouriteIds = await GetFavouriteIdsAsync();

        if (cached != null)
        {
            ApplyFavourites(cached.Data, favouriteIds);
            return cached;
        }

        var (ids, totalCount) =
            await searchService.SearchAsync(query, lang, categoryId, genderId, colorId, sizeId, pageNumber, pageSize);

        var entities = await ProductWithIncludes()
            .Where(x => ids.Contains(x.Id) && !x.IsDeleted)
            .AsNoTracking()
            .ToListAsync();

        var mapped = mapper.Map<List<ProductItemModel>>(entities);

        var ordered = ids
            .Select(id => mapped.FirstOrDefault(e => e.Id == id))
            .Where(e => e != null)
            .Select(e => e!)
            .ToList();

        var localized = ordered.Select(p => Localize(p, lang)).ToList();
        var result = new PageResult<ProductItemModel>();
        if (hasSale == true)
        {
            var data = localized.Where(x => x.Variants.Any(v => v.Sale != null));
            
            result = new PageResult<ProductItemModel>
            {
                Data = data.ToList(),
                TotalCount = data.Count(),
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
        else
        {
            result = new PageResult<ProductItemModel>
            {
                Data = localized,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
        

        await redisService.SetAsync(key, result, TimeSpan.FromMinutes(10));
        ApplyFavourites(result.Data, favouriteIds);
        return result;
    }


    public async Task<ProductItemModel> GetProductById(Guid id, string lang)
    {
        string key = $"product:{id}:{lang}";
        var cache = await redisService.GetAsync<ProductItemModel>(key);

        var favouriteIds = await GetFavouriteIdsAsync();

        if (cache != null)
        {
            cache.IsFavourite = favouriteIds == null ? null : favouriteIds.Contains(cache.Id);
            return cache;
        }

        var product = await context.Products
                          .Where(x => x.Id == id && !x.IsDeleted)
                          .ProjectTo<ProductItemModel>(mapper.ConfigurationProvider)
                          .FirstOrDefaultAsync()
                      ?? throw new Exception("Product not found");

        product = Localize(product, lang);
        await redisService.SetAsync(key, product, TimeSpan.FromMinutes(10));

        product.IsFavourite = favouriteIds == null ? null : favouriteIds.Contains(product.Id);
        return product;
    }

    public async Task<PageResult<ProductItemModel>> GetProductsByStoreId(Guid storeId, string lang,
        int pageNumber = 1, int pageSize = 10)
    {
        string key = $"products:store:{storeId}:{lang}:{pageNumber}:{pageSize}";
        var cached = await redisService.GetAsync<PageResult<ProductItemModel>>(key);
        var favouriteIds = await GetFavouriteIdsAsync();

        if (cached != null)
        {
            ApplyFavourites(cached.Data, favouriteIds);
            return cached;
        }

        var query = context.Products.Where(x => !x.IsDeleted && x.StoreId == storeId);
        var totalCount = await query.CountAsync();

        var products = await ProductWithIncludes()
            .Where(x => !x.IsDeleted && x.StoreId == storeId)
            .OrderBy(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        var mapped = mapper.Map<List<ProductItemModel>>(products);
        var localized = mapped.Select(p => Localize(p, lang)).ToList();

        var result = new PageResult<ProductItemModel>
        {
            Data = localized,
            TotalCount = totalCount,
            CurrentPage = pageNumber,
            PageSize = pageSize
        };

        await redisService.SetAsync(key, result, TimeSpan.FromMinutes(10));
        ApplyFavourites(result.Data, favouriteIds);
        return result;
    }

    public async Task<PageResult<ProductItemModel>> GetProductsByStoreId(Guid storeId, string lang, Guid categoryId,
        int pageNumber = 1, int pageSize = 10)
    {
        string key = $"products:store:{storeId}:category:{categoryId}:{lang}:{pageNumber}:{pageSize}";
        var cached = await redisService.GetAsync<PageResult<ProductItemModel>>(key);
        var favouriteIds = await GetFavouriteIdsAsync();

        if (cached != null)
        {
            ApplyFavourites(cached.Data, favouriteIds);
            return cached;
        }

        var query = context.Products.Where(x => !x.IsDeleted && x.StoreId == storeId && x.CategoryId == categoryId);
        var totalCount = await query.CountAsync();

        var products = await ProductWithIncludes()
            .Where(x => !x.IsDeleted && x.StoreId == storeId && x.CategoryId == categoryId)
            .OrderBy(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        var mapped = mapper.Map<List<ProductItemModel>>(products);
        var localized = mapped.Select(p => Localize(p, lang)).ToList();

        var result = new PageResult<ProductItemModel>
        {
            Data = localized,
            TotalCount = totalCount,
            CurrentPage = pageNumber,
            PageSize = pageSize
        };

        await redisService.SetAsync(key, result, TimeSpan.FromMinutes(10));
        ApplyFavourites(result.Data, favouriteIds);
        return result;
    }

    public async Task<PageResult<ProductItemModel>> GetProductsByStoreId(Guid storeId, string lang, Guid? categoryId, string searchQuery,
        int pageNumber = 1, int pageSize = 10)
    {
        string key = $"products:store:{storeId}:category:{categoryId}:query:{searchQuery}:{lang}:{pageNumber}:{pageSize}";
        var cached = await redisService.GetAsync<PageResult<ProductItemModel>>(key);
        var favouriteIds = await GetFavouriteIdsAsync();

        if (cached != null)
        {
            ApplyFavourites(cached.Data, favouriteIds);
            return cached;
        }

        var (ids, totalCount) = await searchService.SearchAsync(searchQuery, lang, storeId, categoryId, pageNumber, pageSize);

        var entities = await ProductWithIncludes()
            .Where(x => ids.Contains(x.Id) && !x.IsDeleted)
            .AsNoTracking()
            .ToListAsync();

        var mapped = mapper.Map<List<ProductItemModel>>(entities);

        var ordered = ids
            .Select(id => mapped.FirstOrDefault(e => e.Id == id))
            .Where(e => e != null)
            .Select(e => e!)
            .ToList();

        var localized = ordered.Select(p => Localize(p, lang)).ToList();

        var result = new PageResult<ProductItemModel>
        {
            Data = localized,
            TotalCount = totalCount,
            CurrentPage = pageNumber,
            PageSize = pageSize
        };

        await redisService.SetAsync(key, result, TimeSpan.FromMinutes(10));
        ApplyFavourites(result.Data, favouriteIds);
        return result;
    }

    private static ProductItemModel Localize(ProductItemModel model, string lang)
    {
        if (lang.StartsWith("uk", StringComparison.OrdinalIgnoreCase))
        {
            model.Name = model.NameUk ?? model.Name;
            model.Description = model.DescriptionUk ?? model.Description;
            model.Category = model.CategoryUk ?? model.Category;
        }
        return model;
    }
}