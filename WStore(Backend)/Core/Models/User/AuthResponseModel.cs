namespace Core.Models.User;

public class AuthResponseModel
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}