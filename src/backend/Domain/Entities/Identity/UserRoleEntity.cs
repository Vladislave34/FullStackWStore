using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NUlid;

namespace Domain.Entities.Identity;

public class UserRoleEntity : IdentityUserRole<Guid>
{
    public UserEntity User { get; set; } = null!;

    public RoleEntity Role { get; set; } = null!;

}