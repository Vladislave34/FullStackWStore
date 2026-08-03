using Microsoft.AspNetCore.Identity;
using NUlid;

namespace Domain.Entities.Identity;

public class UserEntity : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;
    
    public string Image { get; set; } = null!;
    public string? RefreshToken { get; set; }       
    public long? TelegramChatId { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    
    public DateTime CreateAt { get; set; } =  DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

    public ICollection<UserRoleEntity> UserRoles { get; set; } = null!;
    public CartEntity Carts { get; set; }
    public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
    public ICollection<StoreEntity> Stores { get; set; } = new List<StoreEntity>();
    public ICollection<FeedbackEntity> Feedbacks { get; set; } = new List<FeedbackEntity>();
    public ICollection<ProductEntity> Favourites { get; set; } = new List<ProductEntity>();
    public ICollection<AdrressEntity>? Addresses { get; set; } = new List<AdrressEntity>();
    public ICollection<PaymentEntity>? Payments { get; set; } = new List<PaymentEntity>();

}