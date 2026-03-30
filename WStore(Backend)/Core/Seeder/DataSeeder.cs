using AutoMapper;
using Core.Interfaces;
using Domain;
using Domain.Entities;

namespace Core.Seeder;


public static class DataSeeder
{
    public static async Task SeedAsync(AppStoreContext context, IRedisService redisService, IMapper mapper, IMinioImageService imageService)
    {
        await SeedSizesAsync(context, redisService, mapper);
        await SeedColorsAsync(context, redisService, mapper);
        await SeedCategoriesAsync(context, imageService, redisService); // ← додай
    }

    private static async Task SeedSizesAsync(AppStoreContext context, IRedisService redisService, IMapper mapper)
    {
        if (context.Sizes.Any()) return;

        var sizes = new[]
        {
            new SizeEntity { Name = "XXS" },
            new SizeEntity { Name = "XS" },
            new SizeEntity { Name = "S" },
            new SizeEntity { Name = "M" },
            new SizeEntity { Name = "L" },
            new SizeEntity { Name = "XL" },
            new SizeEntity { Name = "XXL" },
            new SizeEntity { Name = "3XL" },
            new SizeEntity { Name = "36" },
            new SizeEntity { Name = "37" },
            new SizeEntity { Name = "38" },
            new SizeEntity { Name = "39" },
            new SizeEntity { Name = "40" },
            new SizeEntity { Name = "41" },
            new SizeEntity { Name = "42" },
            new SizeEntity { Name = "43" },
            new SizeEntity { Name = "44" },
            new SizeEntity { Name = "45" },
        };

        await context.Sizes.AddRangeAsync(sizes);
        await context.SaveChangesAsync();

        await redisService.RemoveAsync("sizes:all");
    }

    private static async Task SeedColorsAsync(AppStoreContext context, IRedisService redisService, IMapper mapper)
    {
        if (context.Colors.Any()) return;

        var colors = new[]
        {
            new ColorEntity { Name = "Чорний",   Hex = "#000000" },
            new ColorEntity { Name = "Білий",    Hex = "#FFFFFF" },
            new ColorEntity { Name = "Сірий",    Hex = "#808080" },
            new ColorEntity { Name = "Червоний", Hex = "#FF0000" },
            new ColorEntity { Name = "Синій",    Hex = "#0000FF" },
            new ColorEntity { Name = "Зелений",  Hex = "#008000" },
            new ColorEntity { Name = "Жовтий",   Hex = "#FFFF00" },
            new ColorEntity { Name = "Помаранч", Hex = "#FFA500" },
            new ColorEntity { Name = "Рожевий",  Hex = "#FFC0CB" },
            new ColorEntity { Name = "Фіолет",   Hex = "#800080" },
            new ColorEntity { Name = "Бежевий",  Hex = "#F5F5DC" },
            new ColorEntity { Name = "Коричнев", Hex = "#8B4513" },
            new ColorEntity { Name = "Темно-синій", Hex = "#000080" },
            new ColorEntity { Name = "Хакі",     Hex = "#808000" },
        };

        await context.Colors.AddRangeAsync(colors);
        await context.SaveChangesAsync();

        await redisService.RemoveAsync("colors:all");
    }
    private static async Task SeedCategoriesAsync(AppStoreContext context, IMinioImageService imageService, IRedisService redisService)
    {
        if (context.Categories.Any()) return;

        var categories = new[]
        {
            new { Name = "Футболки",   Url = "https://picsum.photos/seed/tshirt/600/600" },
            new { Name = "Штани",      Url = "https://picsum.photos/seed/pants/600/600" },
            new { Name = "Куртки",     Url = "https://picsum.photos/seed/jacket/600/600" },
            new { Name = "Взуття",     Url = "https://picsum.photos/seed/shoes/600/600" },
            new { Name = "Аксесуари",  Url = "https://picsum.photos/seed/access/600/600" },
            new { Name = "Худі",       Url = "https://picsum.photos/seed/hoodie/600/600" },
            new { Name = "Шкарпетки",  Url = "https://picsum.photos/seed/socks/600/600" },
            new { Name = "Сумки",      Url = "https://picsum.photos/seed/bags/600/600" },
        };

        foreach (var cat in categories)
        {
            var imagePath = await imageService.UploadImageFromUrlAsync(cat.Url);
            var entity = new CategoryEntity
            {
                Name = cat.Name,
                image = imagePath
            };
            await context.Categories.AddAsync(entity);
        }

        await context.SaveChangesAsync();
        await redisService.RemoveAsync("categories:all");
    }
}