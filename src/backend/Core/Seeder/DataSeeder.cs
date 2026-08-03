/*
using Amazon.S3;
using Amazon.S3.Model;
using AutoMapper;
using Core.Interfaces;
using Domain;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Seeder;


public static class DataSeeder
{
    

    public static async Task SeedAsync(AppStoreContext context, IRedisService redisService,
        IMapper mapper, IMinioImageService imageService, 
        UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager, IAmazonS3 s3)
    {
        
        await SeedRoles(context, roleManager);
        await SeedAdmin(context, userManager);
        await SeedSizesAsync(context, redisService, mapper);
        await SeedColorsAsync(context, redisService, mapper);
        await SeedBuckets(s3);
        await SeedCategoriesAsync(context, imageService, redisService); 
        await SeedStore(context, imageService, redisService, userManager);
        await SeedGenders(context, redisService);
        await SeedSales(context, redisService);
     
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
            new ColorEntity { Name = "Black",       NameUk = "Чорний",      Hex = "#000000" },
            new ColorEntity { Name = "White",       NameUk = "Білий",       Hex = "#FFFFFF" },
            new ColorEntity { Name = "Gray",        NameUk = "Сірий",       Hex = "#808080" },
            new ColorEntity { Name = "Red",         NameUk = "Червоний",    Hex = "#FF0000" },
            new ColorEntity { Name = "Blue",        NameUk = "Синій",       Hex = "#0000FF" },
            new ColorEntity { Name = "Green",       NameUk = "Зелений",     Hex = "#008000" },
            new ColorEntity { Name = "Yellow",      NameUk = "Жовтий",      Hex = "#FFFF00" },
            new ColorEntity { Name = "Orange",      NameUk = "Помаранчевий",Hex = "#FFA500" },
            new ColorEntity { Name = "Pink",        NameUk = "Рожевий",     Hex = "#FFC0CB" },
            new ColorEntity { Name = "Purple",      NameUk = "Фіолетовий",  Hex = "#800080" },
            new ColorEntity { Name = "Beige",       NameUk = "Бежевий",     Hex = "#F5F5DC" },
            new ColorEntity { Name = "Brown",       NameUk = "Коричневий",  Hex = "#8B4513" },
            new ColorEntity { Name = "Navy",        NameUk = "Темно-синій", Hex = "#000080" },
            new ColorEntity { Name = "Khaki",       NameUk = "Хакі",        Hex = "#808000" },
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

    private static async Task SeedStore(AppStoreContext context, IMinioImageService imageService, IRedisService redisService, UserManager<UserEntity> userManager)
    {
        
        if(context.Stores.Any()) return;
        var admin = await userManager.FindByEmailAsync("ostapchukvladislav77@gmail.com");
        var store = new StoreEntity
        {
            Name = "VladStore",
            Description = "The Best Store and the lowest price",
            OwnerId = admin.Id,
        };
        await context.Stores.AddAsync(store);
        userManager.AddToRoleAsync(admin, "StoreOwner").Wait();
        await context.SaveChangesAsync();
        await redisService.RemoveByPrefixAsync("stores"); 
    }

    private static async Task SeedGenders(AppStoreContext context, IRedisService redisService)
    {
        if(context.Genders.Any()) return;
        
        var list = new List<GenderEntity>
        {
            new GenderEntity() { Name = "Men", NameUk = "Чоловіки" },
            new GenderEntity() {Name = "Women", NameUk = "Жінки"},
            new GenderEntity() {Name = "Unisex", NameUk = "Унісекс"}
        };
        await context.AddRangeAsync(list);
        await context.SaveChangesAsync();
        await redisService.RemoveAsync("genders:all:en");
        await redisService.RemoveAsync("genders:all:uk");
    }

    private static async Task SeedSales(AppStoreContext context, IRedisService redisService)
    {
        if(context.Sales.Any()) return;
        var list = new List<SaleEntity>
        {
            new SaleEntity() { Percent = 5 },
            new SaleEntity() { Percent = 10 },
            new SaleEntity() { Percent = 15 },
            new SaleEntity() { Percent = 20 },
            new SaleEntity() { Percent = 25 },
            new SaleEntity() { Percent = 30 },
            new SaleEntity() { Percent = 40 },
            new SaleEntity() { Percent = 50 },
        };
        await context.AddRangeAsync(list);
        await context.SaveChangesAsync();
        await redisService.RemoveAsync("sales:all");
        await redisService.RemoveAsync("sales:all");
    }

    private static async Task SeedAdmin(AppStoreContext context, UserManager<UserEntity> userManager)
    {
        if (!context.Users.Any())
        {
        
            var adminUser = new UserEntity
            {
                UserName = "admin@gmail.com",
                Email = "ostapchukvladislav77@gmail.com",
                FirstName = "System",
                LastName = "Administrator",
                Image = "default.jpg"
            };
            var result = await userManager.CreateAsync(adminUser, "Admin123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
    
    private static async Task SeedRoles(AppStoreContext context, RoleManager<RoleEntity> roleManager)
    {
        if (!context.Roles.Any())
        {
            var roles = new[] { "Admin", "User", "StoreOwner" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new RoleEntity
                        {
                            Name = role
                        }
                    );
                }


            }
        }
    }

    private static async Task SeedBuckets(IAmazonS3 s3)
    {
        try
        {
            await s3.PutBucketPolicyAsync(new PutBucketPolicyRequest
            {
                BucketName = "wstore-images",
                Policy = """
                         {
                             "Version": "2012-10-17",
                             "Statement": [{
                                 "Effect": "Allow",
                                 "Principal": "*",
                                 "Action": "s3:GetObject",
                                 "Resource": "arn:aws:s3:::wstore-images/*"
                             }]
                         }
                         """
            });
        }
        catch { throw new Exception("Bucket already exists"); }
    }
    
}
*/
using Amazon.S3;
using Amazon.S3.Model;
using AutoMapper;
using Core.Interfaces;
using Domain;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Seeder;


public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var sp = scope.ServiceProvider;

        var context = sp.GetRequiredService<AppStoreContext>();
        var redisService = sp.GetRequiredService<IRedisService>();
        var mapper = sp.GetRequiredService<IMapper>();
        var imageService = sp.GetRequiredService<IMinioImageService>();
        var searchService = sp.GetRequiredService<ISearchService>();
        var userManager = sp.GetRequiredService<UserManager<UserEntity>>();
        var roleManager = sp.GetRequiredService<RoleManager<RoleEntity>>();
        var s3 = sp.GetRequiredService<IAmazonS3>();

        await context.Database.MigrateAsync();

        
        var seeder = new SeederRunner(context, redisService, mapper, imageService, searchService, userManager, roleManager, s3);
        await seeder.RunAsync();
    }
}


internal sealed class SeederRunner
{
    private readonly AppStoreContext _context;
    private readonly IRedisService _redisService;
    private readonly IMapper _mapper;
    private readonly IMinioImageService _imageService;
    private readonly ISearchService _searchService;
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<RoleEntity> _roleManager;
    private readonly IAmazonS3 _s3;

    public SeederRunner(
        AppStoreContext context,
        IRedisService redisService,
        IMapper mapper,
        IMinioImageService imageService,
        ISearchService searchService,
        UserManager<UserEntity> userManager,
        RoleManager<RoleEntity> roleManager,
        IAmazonS3 s3)
    {
        _context = context;
        _redisService = redisService;
        _mapper = mapper;
        _imageService = imageService;
        _searchService = searchService;
        _userManager = userManager;
        _roleManager = roleManager;
        _s3 = s3;
    }

    public async Task RunAsync()
    {
        await SeedRoles();
        await SeedAdmin();
        await SeedSizesAsync();
        await SeedColorsAsync();
        await SeedBuckets();
        await SeedCategoriesAsync();
        await SeedStore();
        await SeedGenders();
        await SeedSales();
        await SeedSearch();

    }

    private async Task SeedSizesAsync()
    {
        if (await _context.Sizes.AnyAsync()) return;

        var sizes = new[]
        {
            "XXS", "XS", "S", "M", "L", "XL", "XXL", "3XL",
            "36", "37", "38", "39", "40", "41", "42", "43", "44", "45"
        }.Select(name => new SizeEntity { Name = name });

        await _context.Sizes.AddRangeAsync(sizes);
        await _context.SaveChangesAsync();
        await _redisService.RemoveAsync("sizes:all");
    }

    private async Task SeedColorsAsync()
    {
        if (await _context.Colors.AnyAsync()) return;

        var colors = new[]
        {
            new ColorEntity { Name = "Black",  NameUk = "Чорний",       Hex = "#000000" },
            new ColorEntity { Name = "White",  NameUk = "Білий",        Hex = "#FFFFFF" },
            new ColorEntity { Name = "Gray",   NameUk = "Сірий",        Hex = "#808080" },
            new ColorEntity { Name = "Red",    NameUk = "Червоний",     Hex = "#FF0000" },
            new ColorEntity { Name = "Blue",   NameUk = "Синій",        Hex = "#0000FF" },
            new ColorEntity { Name = "Green",  NameUk = "Зелений",      Hex = "#008000" },
            new ColorEntity { Name = "Yellow", NameUk = "Жовтий",       Hex = "#FFFF00" },
            new ColorEntity { Name = "Orange", NameUk = "Помаранчевий", Hex = "#FFA500" },
            new ColorEntity { Name = "Pink",   NameUk = "Рожевий",      Hex = "#FFC0CB" },
            new ColorEntity { Name = "Purple", NameUk = "Фіолетовий",   Hex = "#800080" },
            new ColorEntity { Name = "Beige",  NameUk = "Бежевий",      Hex = "#F5F5DC" },
            new ColorEntity { Name = "Brown",  NameUk = "Коричневий",   Hex = "#8B4513" },
            new ColorEntity { Name = "Navy",   NameUk = "Темно-синій",  Hex = "#000080" },
            new ColorEntity { Name = "Khaki",  NameUk = "Хакі",         Hex = "#808000" },
        };

        await _context.Colors.AddRangeAsync(colors);
        await _context.SaveChangesAsync();
        await _redisService.RemoveAsync("colors:all");
    }

    private async Task SeedCategoriesAsync()
    {
        if (await _context.Categories.AnyAsync()) return;

        var categories = new[]
        {
            new { Name = "T-Shirts",    NameUk = "Футболки",  Url = "https://picsum.photos/seed/tshirt/600/600" },
            new { Name = "Pants",       NameUk = "Штани",     Url = "https://picsum.photos/seed/pants/600/600" },
            new { Name = "Jackets",     NameUk = "Куртки",    Url = "https://picsum.photos/seed/jacket/600/600" },
            new { Name = "Shoes",       NameUk = "Взуття",    Url = "https://picsum.photos/seed/shoes/600/600" },
            new { Name = "Accessories", NameUk = "Аксесуари", Url = "https://picsum.photos/seed/access/600/600" },
            new { Name = "Hoodies",     NameUk = "Худі",      Url = "https://picsum.photos/seed/hoodie/600/600" },
            new { Name = "Socks",       NameUk = "Шкарпетки", Url = "https://picsum.photos/seed/socks/600/600" },
            new { Name = "Bags",        NameUk = "Сумки",     Url = "https://picsum.photos/seed/bags/600/600" },
        };

        foreach (var cat in categories)
        {
            var imagePath = await _imageService.UploadImageFromUrlAsync(cat.Url);
            await _context.Categories.AddAsync(new CategoryEntity
            {
                Name = cat.Name,
                NameUk = cat.NameUk,
                image = imagePath
            });
        }

        await _context.SaveChangesAsync();
        await _redisService.RemoveAsync("categories:all:en");
        await _redisService.RemoveAsync("categories:all:uk");
    }
    

    private async Task SeedStore()
    {
        if (await _context.Stores.AnyAsync()) return;

        var admin = await _userManager.FindByEmailAsync("ostapchukvladislav77@gmail.com");
        if (admin == null) return; 

        var store = new StoreEntity
        {
            Name = "VladStore",
            Description = "The Best Store and the lowest price",
            OwnerId = admin.Id,
        };

        await _context.Stores.AddAsync(store);
        await _userManager.AddToRoleAsync(admin, "StoreOwner");
        await _context.SaveChangesAsync();
        await _redisService.RemoveByPrefixAsync("stores");
    }

    private async Task SeedGenders()
    {
        if (await _context.Genders.AnyAsync()) return;

        var list = new List<GenderEntity>
        {
            new() { Name = "Men",    NameUk = "Чоловіки" },
            new() { Name = "Women",  NameUk = "Жінки" },
            new() { Name = "Unisex", NameUk = "Унісекс" },
        };

        await _context.AddRangeAsync(list);
        await _context.SaveChangesAsync();
        await _redisService.RemoveAsync("genders:all:en");
        await _redisService.RemoveAsync("genders:all:uk");
    }

    private async Task SeedSales()
    {
        if (await _context.Sales.AnyAsync()) return;

        var list = new List<SaleEntity>
        {
            new() { Percent = 5 }, new() { Percent = 10 }, new() { Percent = 15 }, new() { Percent = 20 },
            new() { Percent = 25 }, new() { Percent = 30 }, new() { Percent = 40 }, new() { Percent = 50 },
        };

        await _context.AddRangeAsync(list);
        await _context.SaveChangesAsync();
        await _redisService.RemoveAsync("sales:all");
    }

    private async Task SeedAdmin()
    {
        if (await _context.Users.AnyAsync()) return;

        var adminUser = new UserEntity
        {
            UserName = "admin@gmail.com",
            Email = "ostapchukvladislav77@gmail.com",
            FirstName = "System",
            LastName = "Administrator",
            Image = "default.jpg"
        };

        var result = await _userManager.CreateAsync(adminUser, "Admin123");
        if (result.Succeeded)
            await _userManager.AddToRoleAsync(adminUser, "Admin");
        var entity = new CartEntity()
        {
            UserId = adminUser.Id,
        };
        await _context.Carts.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    private async Task SeedRoles()
    {
        var roles = new[] { "Admin", "User", "StoreOwner" };
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new RoleEntity { Name = role });
        }
    }

    private async Task SeedBuckets()
    {
        try
        {
            await _s3.PutBucketPolicyAsync(new PutBucketPolicyRequest
            {
                BucketName = "wstore-images",
                Policy = """
                         {
                             "Version": "2012-10-17",
                             "Statement": [{
                                 "Effect": "Allow",
                                 "Principal": "*",
                                 "Action": "s3:GetObject",
                                 "Resource": "arn:aws:s3:::wstore-images/*"
                             }]
                         }
                         """
            });
        }
        catch(AmazonS3Exception exception)
        {
            Console.WriteLine(exception.ToString());
        }
    }

    private async Task SeedSearch()
    {
        await _searchService.EnsureIndexCreatedAsync();
        await _searchService.ReindexAllAsync();
    }
}