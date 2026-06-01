using Core.Models.Search;
using Core.Models.User;

namespace Core.Interfaces;

public interface IUserService
{
    Task<UserItemModel> GetUserProfileAsync();
    public Task<bool> ForgotPasswordAsync(ForgotPasswordModel model);
    public Task<bool> ResetPasswordAsync(ResetPasswordModel model);
    public Task<AuthResponseModel> EditProfileAsync(EditProfileModel model);

    //public Task<SearchResult<UserItemModel>> SearchAsync(UserSearchModel model);
}