namespace Core.Models.User;

public class LoginModel
{
    /// <summary>
    /// Електронна пошта користувача
    /// </summary>
    /// <example>ostapchuk_vladyslav@gymnasia21.lutsk.ua</example>
    public string Email { get; set; } = null!;
    /// <summary>
    /// Пароль користувача
    /// </summary>
    /// <example>Admin123</example>
    public string Password { get; set; } = null!;
}