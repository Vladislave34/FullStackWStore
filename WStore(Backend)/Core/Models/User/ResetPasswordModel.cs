namespace Core.Models.User;

public class ResetPasswordModel
{
    public string Email { get; set; } = String.Empty;
    public string Token { get; set; } = String.Empty;
    public string NewPassword { get; set; } = String.Empty;
    public string ConfirmNewPassword { get; set; } = String.Empty;
}