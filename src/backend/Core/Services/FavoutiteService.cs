using AutoMapper;
using Core.Interfaces;
using Core.Models.Product;
using Domain;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class FavoutiteService(
    AppStoreContext context,
    IMapper mapper,
    IAuthService authService,
    IRedisService redisService) : IFavoutiteService
{
    private Task<Guid> userId => authService.GetUserIdAsync();

    public async Task AddFavourite(Guid favouriteId)
    {
        var currentUserId = await userId;

        var alreadyExists = await context.Users
            .Where(u => u.Id == currentUserId)
            .SelectMany(u => u.Favourites)
            .AnyAsync(p => p.Id == favouriteId);

        if (alreadyExists) return;

        var user = await context.Users
            .FirstOrDefaultAsync(p => p.Id == currentUserId)
                ?? throw new Exception("User not found");

        var product = await context.Products
            .FirstOrDefaultAsync(p => p.Id == favouriteId && !p.IsDeleted)
                ?? throw new Exception("Product not found");

        context.Attach(user);
        context.Attach(product);
        user.Favourites.Add(product);
        await context.SaveChangesAsync();

        await ClearProductCache(product.Id, product.StoreId);
    }

    public async Task RemoveFavourite(Guid favouriteId)
    {
        var currentUserId = await userId;

        var user = await context.Users
            .Include(u => u.Favourites.Where(p => p.Id == favouriteId))
            .FirstOrDefaultAsync(u => u.Id == currentUserId)
                ?? throw new Exception("User not found");

        var product = user.Favourites.FirstOrDefault(p => p.Id == favouriteId);
        if (product == null) return;

        user.Favourites.Remove(product);
        await context.SaveChangesAsync();

        await ClearProductCache(product.Id, product.StoreId);
    }

    public async Task<List<ProductItemModel>> GetFavourites(string lang)
    {
        var currentUserId = await userId;

        var products = await context.Users
            .Where(u => u.Id == currentUserId)
            .SelectMany(u => u.Favourites)
            .Where(p => !p.IsDeleted)
            .Include(p=>p.GenderEntity)
            .Include(p => p.CategoryEntity)
            .Include(p => p.Variants).ThenInclude(v => v.Color)
            .Include(p => p.Variants).ThenInclude(v => v.Size)
            .Include(p => p.Variants).ThenInclude(v => v.Image.Where(i => !i.IsDeleted))
            .AsNoTracking()
            .ToListAsync();

        var mapped = mapper.Map<List<ProductItemModel>>(products);
        var localized = mapped.Select(p => Localize(p, lang)).ToList();

        foreach (var p in localized) p.IsFavourite = true;

        return localized;
    }

    public async Task<bool> IsFavourite(Guid favouriteId)
    {
        var currentUserId = await userId;

        return await context.Users
            .Where(u => u.Id == currentUserId)
            .SelectMany(u => u.Favourites)
            .AnyAsync(p => p.Id == favouriteId);
    }

    private async Task ClearProductCache(Guid productId, Guid storeId)
    {
        await redisService.RemoveAsync($"product:{productId}:en");
        await redisService.RemoveAsync($"product:{productId}:uk");
        await redisService.RemoveByPrefixAsync("products:all:");
        await redisService.RemoveByPrefixAsync($"products:store:{storeId}:");
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