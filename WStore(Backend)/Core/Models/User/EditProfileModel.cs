using Microsoft.AspNetCore.Http;

namespace Core.Models.User;

public class EditProfileModel
{
    public string? FirstName { get; set; } 
    public string? LastName { get; set; }
    public IFormFile? Image { get; set; }
    public string? UserName { get; set; }
    
}