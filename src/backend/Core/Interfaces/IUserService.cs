using Core.Models.Search;
using Core.Models.User;

namespace Core.Interfaces;

public interface IUserService
{
    Task<UserItemModel> GetUserProfileAsync();
    Task<bool> ForgotPasswordAsync(ForgotPasswordModel model);
    Task<bool> ResetPasswordAsync(ResetPasswordModel model);
    Task<AuthResponseModel> EditProfileAsync(EditProfileModel model);
    
    Task<AuthResponseModel> Register(RegisterModel model);
    Task<AuthResponseModel> Login(LoginModel model);
    Task LinkTelegram(LinkTelegramModel model);
    
    

    //public Task<SearchResult<UserItemModel>> SearchAsync(UserSearchModel model);
}