using Microsoft.AspNetCore.Identity;
using NUlid;

namespace Domain.Entities.Identity;

public class RoleEntity : IdentityRole<Guid>
{
    public RoleEntity()
    {
    }
    public RoleEntity(string Name) {this.Name =  Name;}

    public ICollection<UserRoleEntity> UserRoles { get; set; } = null!;
}