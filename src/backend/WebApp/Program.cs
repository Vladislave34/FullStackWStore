using System.Text;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using AutoMapper;

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
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Nest;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Logger for startup-time errors, before the main DI container is fully built
using var startupLoggerFactory = LoggerFactory.Create(cfg => cfg.AddConsole());
var startupLogger = startupLoggerFactory.CreateLogger("Startup");

// Add services
builder.Services.AddEndpointsApiExplorer();

try
{
    builder.Services.AddDbContext<AppStoreContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}
catch (Exception ex)
{
    startupLogger.LogCritical(ex, "Failed to configure PostgreSQL connection (DefaultConnection).");
    throw;
}

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
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});


try
{
    var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
    if (string.IsNullOrWhiteSpace(redisConnectionString))
        throw new InvalidOperationException("Redis connection string is not set in configuration.");

    var redisMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
    builder.Services.AddSingleton<IConnectionMultiplexer>(redisMultiplexer);
}
catch (Exception ex)
{
    startupLogger.LogCritical(ex, "Failed to connect to Redis.");
    throw;
}


try
{
    var minioConfig = builder.Configuration.GetSection("MinIO");
    var accessKey = minioConfig["AccessKey"];
    var secretKey = minioConfig["SecretKey"];
    var endpoint = minioConfig["Endpoint"];

    if (string.IsNullOrWhiteSpace(accessKey) || string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(endpoint))
        throw new InvalidOperationException("MinIO configuration section is incomplete (AccessKey/SecretKey/Endpoint).");

    var s3Client = new AmazonS3Client(
        new BasicAWSCredentials(accessKey, secretKey),
        new AmazonS3Config
        {
            ServiceURL = endpoint,
            ForcePathStyle = true
        }
    );

    builder.Services.AddSingleton<IAmazonS3>(s3Client);
}
catch (Exception ex)
{
    startupLogger.LogCritical(ex, "Failed to configure MinIO/S3 client.");
    throw;
}


try
{
    var connectionString = builder.Configuration.GetConnectionString("ElasticSearch");
    var settings = new ConnectionSettings(new Uri(connectionString))
        .DefaultIndex("products");
    var elasticClient = new ElasticClient(settings);

    builder.Services.AddSingleton<IElasticClient>(elasticClient);
}
catch (Exception ex)
{
    startupLogger.LogCritical(ex, "Failed to configure Elasticsearch client.");
    throw;
}

builder.Services.AddScoped<IMinioImageService, MinioImageService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<ISizeService, SizeService>();
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
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
builder.Services.AddScoped<IProductVariantServices, ProductVariantService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddSingleton<ITelegramNotificationService, TelegramNotificationService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IGenderService, GenderService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IFavoutiteService, FavoutiteService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IStatisticService, StatisticService>();
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

try
{
    var jwtKey = builder.Configuration["Jwt:Key"];
    if (string.IsNullOrWhiteSpace(jwtKey))
        throw new InvalidOperationException("Jwt:Key is not set in configuration.");

    builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });
}
catch (Exception ex)
{
    startupLogger.LogCritical(ex, "Failed to configure JWT authentication.");
    throw;
}

var app = builder.Build();


app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("GlobalExceptionHandler");

        var exceptionFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        logger.LogError(exceptionFeature?.Error, "Unhandled exception while processing request {Path}", context.Request.Path);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { error = "Internal server error." });
    });
});

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

try
{
    var dirImageName = builder.Configuration.GetValue<string>("DirImageName") ?? "images";
    var path = Path.Combine(Directory.GetCurrentDirectory(), dirImageName);
    Directory.CreateDirectory(dirImageName);
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(path),
        RequestPath = $"/{dirImageName}"
    });
}
catch (Exception ex)
{
    startupLogger.LogCritical(ex, "Failed to configure static files (images directory).");
    throw;
}

if (app.Environment.IsDevelopment())
{
    try
    {
        await DataSeeder.SeedAsync(app.Services);
    }
    catch (Exception ex)
    {
        startupLogger.LogError(ex, "Error while seeding the database (DataSeeder.SeedAsync).");
      
    }
}

try
{
    app.Run();
}
catch (Exception ex)
{
    startupLogger.LogCritical(ex, "Application terminated unexpectedly.");
    throw;
}