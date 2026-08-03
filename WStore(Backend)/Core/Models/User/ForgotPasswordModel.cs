namespace Core.Models.User;

public sealed record ForgotPasswordModel
{
    public string Email { get; set; } = String.Empty;
}