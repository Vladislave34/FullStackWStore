namespace Core.Models.User;

public class UserItemModel
{
    public Guid Id { get; set; } 
    public string FullName { get; set; } = String.Empty;
    
    public string Phone { get; set; } = String.Empty;
    
    public string Image { get; set; } = String.Empty;

    public string Email { get; set; } = String.Empty;

    public List<string> Roles = new();


}