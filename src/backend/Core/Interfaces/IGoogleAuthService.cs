using Core.Models.Color;
using Core.Models.User;

namespace Core.Interfaces;

public interface IGoogleAuthService
{
    Task<AuthResponseModel> LoginByGoogleAsync(string token);
}