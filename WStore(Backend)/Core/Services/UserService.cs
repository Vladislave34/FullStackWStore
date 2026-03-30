using AutoMapper;
using Core.Interfaces;
using Core.Models.User;
using Domain;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.Services;

public class UserService(IAuthService authService, IMapper mapper, AppStoreContext appStoreContext,  IConfiguration configuration,
    IEmailSender smtpService, UserManager<UserEntity> userManager) : IUserService
{
    public async Task<UserItemModel> GetUserProfileAsync()
    {
        var userId = await authService.GetUserIdAsync();
        var user = await appStoreContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        var profile = mapper.Map<UserItemModel>(user);
        return profile;
        
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return false;
        }
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = $"{configuration["ClientUrl"]}/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(model.Email)}";
        var To = model.Email;
        var Subject = "Reset Password";
        var Body = $"<p>Click the link below to reset your password:</p><a href='{resetLink}'>Reset Password</a>";
        await smtpService.SendEmailAsync(To, Subject, Body);
        return true;

    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            var res = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!res.Succeeded)
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        return true;
    }
}