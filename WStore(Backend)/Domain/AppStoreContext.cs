


using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NUlid;

namespace Domain;
public class AppStoreContext 
    : IdentityDbContext
    <
    UserEntity,
    RoleEntity, 
    Guid,
    IdentityUserClaim<Guid>,
    UserRoleEntity,
    IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>,
    IdentityUserToken<Guid>
    >
{
    
    public AppStoreContext(DbContextOptions<AppStoreContext> options) : base(options)
    {
       
    }
    public DbSet<CategoryEntity>  Categories { get; set; }
    public DbSet<ProductEntity>  Products { get; set; }
    public DbSet<StoreEntity>  Stores { get; set; }
    public DbSet<ProductVariantEntity>  ProductVariants { get; set; }
    public DbSet<ColorEntity>  Colors { get; set; }
    public DbSet<SizeEntity>  Sizes { get; set; }
    public DbSet<CartEntity>  Carts { get; set; }
    public DbSet<OrderEntity>  Orders { get; set; }
    public DbSet<CartItemEntity>  CartItems { get; set; }
    public DbSet<OrderItemEntity>  OrderItems { get; set; }
    public DbSet<ProductVariantImageEntity>  ProductVariantImages { get; set; }
    public DbSet<StoreImageEntity>  StoreImages { get; set; }
    public DbSet<FeedbackImageEntity>  FeedbackImages { get; set; }
    public DbSet<OrderHistoryEntity>  OrderHistories { get; set; }
    public DbSet<OrderStatusEntity>  OrderStatuses { get; set; }
    public DbSet<FeedbackEntity>   Feedbacks { get; set; }
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Для UserRoleEntity комбінований ключ
        builder.Entity<UserRoleEntity>(b =>
        {
            b.HasKey(ur => new { ur.UserId, ur.RoleId });

            b.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            b.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });
        
        
        
        


    }
}