using AutoMapper;
using Core.Interfaces;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Seeder;


public static class DataSeeder
{
    public static async Task SeedAsync(AppStoreContext context, IRedisService redisService, IMapper mapper, IMinioImageService imageService, ISearchService searchService)
    {
        await SeedSizesAsync(context, redisService, mapper);
        await SeedColorsAsync(context, redisService, mapper);
        await SeedCategoriesAsync(context, imageService, redisService); 
        await SeedProductsAsync(context, redisService, searchService);
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
            new { Name = "T-Shirts",     NameUk = "Футболки",    Url = "https://picsum.photos/seed/tshirt/600/600" },
            new { Name = "Pants",        NameUk = "Штани",       Url = "https://picsum.photos/seed/pants/600/600" },
            new { Name = "Jackets",      NameUk = "Куртки",      Url = "https://picsum.photos/seed/jacket/600/600" },
            new { Name = "Shoes",        NameUk = "Взуття",      Url = "https://picsum.photos/seed/shoes/600/600" },
            new { Name = "Accessories",  NameUk = "Аксесуари",   Url = "https://picsum.photos/seed/access/600/600" },
            new { Name = "Hoodies",      NameUk = "Худі",        Url = "https://picsum.photos/seed/hoodie/600/600" },
            new { Name = "Socks",        NameUk = "Шкарпетки",   Url = "https://picsum.photos/seed/socks/600/600" },
            new { Name = "Bags",         NameUk = "Сумки",       Url = "https://picsum.photos/seed/bags/600/600" },
        };

        foreach (var cat in categories)
        {
            var imagePath = await imageService.UploadImageFromUrlAsync(cat.Url);
            var entity = new CategoryEntity
            {
                Name = cat.Name,
                NameUk = cat.NameUk,
                image = imagePath
            };
            await context.Categories.AddAsync(entity);
        }

        await context.SaveChangesAsync();
        await redisService.RemoveAsync("categories:all:en");
        await redisService.RemoveAsync("categories:all:uk");
    }
        private static async Task SeedProductsAsync(AppStoreContext context, IRedisService redisService, ISearchService searchService)
    {
        if (context.Products.Any()) return;

        var categories = context.Categories.ToList();
        var store = context.Stores.FirstOrDefault();

        if (store == null || !categories.Any()) return;

        var getCategoryId = (string name) =>
            categories.FirstOrDefault(c => c.Name == name)?.Id ?? categories.First().Id;

        var products = new[]
        {
            new { Name = "Basic White T-Shirt",  NameUk = "Базова біла футболка",   Description = "Classic white t-shirt for everyday wear",     DescriptionUk = "Класична біла футболка на кожен день",        Category = "T-Shirts" },
            new { Name = "Black T-Shirt",         NameUk = "Чорна футболка",         Description = "Minimalist black t-shirt",                     DescriptionUk = "Мінімалістична чорна футболка",               Category = "T-Shirts" },
            new { Name = "Slim Fit Pants",        NameUk = "Вузькі штани",           Description = "Modern slim fit pants for any occasion",       DescriptionUk = "Сучасні вузькі штани для будь-якого випадку", Category = "Pants" },
            new { Name = "Cargo Pants",           NameUk = "Карго штани",            Description = "Comfortable cargo pants with pockets",         DescriptionUk = "Зручні карго штани з кишенями",               Category = "Pants" },
            new { Name = "Winter Jacket",         NameUk = "Зимова куртка",          Description = "Warm winter jacket for cold weather",          DescriptionUk = "Тепла зимова куртка для холодної погоди",     Category = "Jackets" },
            new { Name = "Denim Jacket",          NameUk = "Джинсова куртка",        Description = "Classic denim jacket",                         DescriptionUk = "Класична джинсова куртка",                    Category = "Jackets" },
            new { Name = "White Sneakers",        NameUk = "Білі кросівки",          Description = "Clean white sneakers for everyday use",        DescriptionUk = "Чисті білі кросівки для щоденного носіння",   Category = "Shoes" },
            new { Name = "Black Boots",           NameUk = "Чорні черевики",         Description = "Durable black leather boots",                  DescriptionUk = "Міцні чорні шкіряні черевики",                Category = "Shoes" },
            new { Name = "Leather Belt",          NameUk = "Шкіряний ремінь",        Description = "Classic leather belt",                         DescriptionUk = "Класичний шкіряний ремінь",                   Category = "Accessories" },
            new { Name = "Wool Scarf",            NameUk = "Вовняний шарф",          Description = "Soft wool scarf for winter",                   DescriptionUk = "М'який вовняний шарф для зими",               Category = "Accessories" },
            new { Name = "Pullover Hoodie",       NameUk = "Худі пуловер",           Description = "Comfortable pullover hoodie",                  DescriptionUk = "Зручне худі пуловер",                         Category = "Hoodies" },
            new { Name = "Zip-Up Hoodie",         NameUk = "Худі на блискавці",      Description = "Classic zip-up hoodie",                        DescriptionUk = "Класичне худі на блискавці",                  Category = "Hoodies" },
            new { Name = "Cotton Socks",          NameUk = "Бавовняні шкарпетки",    Description = "Soft cotton everyday socks",                   DescriptionUk = "М'які бавовняні шкарпетки на кожен день",     Category = "Socks" },
            new { Name = "Sport Socks",           NameUk = "Спортивні шкарпетки",    Description = "Breathable sport socks",                       DescriptionUk = "Дихаючі спортивні шкарпетки",                 Category = "Socks" },
            new { Name = "Leather Backpack",      NameUk = "Шкіряний рюкзак",        Description = "Stylish leather backpack",                     DescriptionUk = "Стильний шкіряний рюкзак",                    Category = "Bags" },
            new { Name = "Tote Bag",              NameUk = "Сумка-шопер",            Description = "Minimalist canvas tote bag",                   DescriptionUk = "Мінімалістична сумка-шопер з канвасу",        Category = "Bags" },
        };

        foreach (var p in products)
        {
            var entity = new ProductEntity
            {
                Name = p.Name,
                NameUk = p.NameUk,
                Description = p.Description,
                DescriptionUk = p.DescriptionUk,
                CategoryId = getCategoryId(p.Category),
                StoreId = store.Id,
            };
            await context.Products.AddAsync(entity);
        }

        await context.SaveChangesAsync();

        // індексуємо в Elastic
        var savedProducts = await context.Products
            .Include(x => x.CategoryEntity)
            .Include(x => x.Store)
            .Include(x => x.Variants).ThenInclude(v => v.Color)
            .Include(x => x.Variants).ThenInclude(v => v.Size)
            .ToListAsync();

        foreach (var product in savedProducts)
            await searchService.IndexProductAsync(product);

        await redisService.RemoveAsync("products:all:en");
        await redisService.RemoveAsync("products:all:uk");
    }
    
}