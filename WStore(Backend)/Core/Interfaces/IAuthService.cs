using Core.Models.User;

namespace Core.Interfaces;

public interface IAuthService
{
    Task<Guid> GetUserIdAsync();
}