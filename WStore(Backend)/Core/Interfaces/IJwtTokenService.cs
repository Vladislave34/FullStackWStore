using Core.Models.User;
using Domain.Entities.Identity;

namespace Core.Interfaces;

public interface IJwtTokenService
{
    Task<string> CreateToken(UserEntity user);
    string GenerateRefreshToken();
    Task<AuthResponseModel> CreateAuthResponse(UserEntity user);
    Task<AuthResponseModel> RefreshTokenAsync(string refreshToken);
}