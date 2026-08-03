namespace Core.Models.User;

public sealed record GoogleLoginRequestModel
{
    public string IdToken { get; set; } = null!;
}