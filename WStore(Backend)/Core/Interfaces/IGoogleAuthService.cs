using Core.Models.Color;

namespace Core.Interfaces;

public interface IGoogleAuthService
{
    Task<string> LoginByGoogleAsync(string token);
}