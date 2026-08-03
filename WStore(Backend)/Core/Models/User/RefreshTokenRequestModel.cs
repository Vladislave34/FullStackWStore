namespace Core.Models.User;

public sealed record RefreshTokenRequestModel
{
    public string RefreshToken { get; set; }
}