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
    public ICollection<CartEntity> Carts { get; set; } = new List<CartEntity>();
    public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
    public ICollection<StoreEntity> Stores { get; set; } = new List<StoreEntity>();
    public ICollection<FeedbackEntity> Feedbacks { get; set; } = new List<FeedbackEntity>();

}