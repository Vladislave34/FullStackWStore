using System.Text;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using AutoMapper;
using Core.Interceptors;
using Core.Interfaces;
using Core.Models.ProductVariantImage;
using Core.Models.StoreImage;
using Core.Models.User;
using Core.Seeder;
using Core.Services;
using Domain;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nest;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppStoreContext>(
    options => 
        options
            .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            .AddInterceptors(new OrderHistoryInterceptor()));

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var assemblyName = typeof(LoginModel).Assembly.GetName().Name;
builder.Services.AddSwaggerGen(opt =>
{
    //var fileDoc = $"{assemblyName}.xml";
    //var filePath = Path.Combine(AppContext.BaseDirectory, fileDoc);
    //opt.IncludeXmlComments(filePath);

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });

});

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));
var minioConfig = builder.Configuration.GetSection("MinIO");
var s3Client = new AmazonS3Client(
    new BasicAWSCredentials(
        minioConfig["AccessKey"],
        minioConfig["SecretKey"]
    ),
    new AmazonS3Config
    {
        ServiceURL = minioConfig["Endpoint"],
        ForcePathStyle = true  
    }
);
var settings = new ConnectionSettings(new Uri("http://192.168.64.3:9200"))
    .DefaultIndex("products");
var elasticClient = new ElasticClient(settings);

builder.Services.AddSingleton<IAmazonS3>(s3Client);
builder.Services.AddSingleton<IElasticClient>(elasticClient);
builder.Services.AddScoped<IMinioImageService, MinioImageService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<ISizeService, SizeService>();
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
//builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IStoreImageService, StoreImageService>();
builder.Services.AddScoped<
    IEntityImageService<StoreImageEntity, StoreImageAddUpdateModel, StoreImageItemModel>,
    EntityImageService<StoreImageEntity, StoreImageAddUpdateModel, StoreImageItemModel>>();
builder.Services.AddScoped<
    IEntityImageService<ProductVariantImageEntity, ProductVariantImageAddModel, ProductVariantImageItemModel>,
    EntityImageService<ProductVariantImageEntity, ProductVariantImageAddModel, ProductVariantImageItemModel>>();
builder.Services.AddScoped<
    IEntityImageService<FeedbackImageEntity, FeedbackImageAddModel, FeedbackImageItemModel>,
    EntityImageService<FeedbackImageEntity, FeedbackImageAddModel, FeedbackImageItemModel>>();
builder.Services.AddScoped<IProductVariantServices,  ProductVariantService>();
builder.Services.AddScoped<IFeedbackService,  FeedbackService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddSingleton<ITelegramNotificationService, TelegramNotificationService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});





builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;
    })
    .AddEntityFrameworkStores<AppStoreContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            
        }
    
    )
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });





var app = builder.Build();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

var dirImageName = builder.Configuration.GetValue<string>("DirImageName") ?? "images";
var path = Path.Combine(Directory.GetCurrentDirectory(), dirImageName);
Directory.CreateDirectory(dirImageName);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(path),
    RequestPath = $"/{dirImageName}"
});
using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppStoreContext>();
    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
    
    dbContext.Database.Migrate();
    var redisService = serviceScope.ServiceProvider.GetRequiredService<IRedisService>();
    var mapper = serviceScope.ServiceProvider.GetRequiredService<IMapper>();
    var imageService = serviceScope.ServiceProvider.GetRequiredService<IMinioImageService>();
    await DataSeeder.SeedAsync(dbContext, redisService, mapper, imageService);
    var roles = new[] {"Admin",  "User", "StoreOwner"};
    var s3 = serviceScope.ServiceProvider.GetRequiredService<IAmazonS3>();
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
    catch { Console.WriteLine("exist"); }
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
    if (!dbContext.Users.Any())
    {
        var userManager = serviceScope.ServiceProvider
            .GetRequiredService<UserManager<UserEntity>>();
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

    if (!dbContext.OrderStatuses.Any())
    {
        var statuses = new[]
        {
            new OrderStatusEntity {  Name = "Pending" },
            new OrderStatusEntity {  Name = "Confirmed" },
            new OrderStatusEntity {  Name = "Processing" },
            new OrderStatusEntity {  Name = "Shipped" },
            new OrderStatusEntity {  Name = "Delivered" },
            new OrderStatusEntity {  Name = "Cancelled" },
            new OrderStatusEntity {  Name = "Refunded" },
            new OrderStatusEntity {  Name = "Failed" },
        };
        await dbContext.OrderStatuses.AddRangeAsync(statuses);
        await dbContext.SaveChangesAsync(); 
    }
}

app.Run();

