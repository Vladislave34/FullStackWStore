using AutoMapper;
using Core.Interfaces;
using Core.Models.User;
using Domain;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.Services;

public class UserService(IAuthService authService, IMapper mapper, AppStoreContext appStoreContext,
    IConfiguration configuration, IEmailSender smtpService, UserManager<UserEntity> userManager,
    IMinioImageService minioImageService, IJwtTokenService jwtTokenService, RoleManager<RoleEntity> roleManager, 
    IMinioImageService imageService
    ) : IUserService
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
    
    public async Task<AuthResponseModel> EditProfileAsync(EditProfileModel model)
    {
        var userId = await authService.GetUserIdAsync();
        Console.WriteLine(userId);
        var entity = await appStoreContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (entity == null)
        {
            throw new Exception("User not found");
        }
        Console.WriteLine(entity.LastName);
        if (entity == null)
        {
            throw new Exception("User not found");
        }
        mapper.Map(model, entity);
        if (model.Image != null)
            entity.Image = await minioImageService.UpdateImageAsync(entity.Image, model.Image);
        await appStoreContext.SaveChangesAsync();
        var response = await jwtTokenService.CreateAuthResponse(entity);
        return response;
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return false;
        }
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = $"{configuration["ClientUrl"]}/en/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(model.Email)}";
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

    public async Task<AuthResponseModel> Login(LoginModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
            throw new Exception("Invalid email or password.");

        var response = await jwtTokenService.CreateAuthResponse(user);
        return response;
    }

    public async Task<AuthResponseModel> Register(RegisterModel model)
    {
        var existingUser = await userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
            throw new Exception("User already exists.");

        var user = new UserEntity
        {
            UserName = model.UserName,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Image = await imageService.UploadImageAsync(model.Image)
        };

        var res = await userManager.CreateAsync(user, model.Password);
        if (!res.Succeeded)
            throw new Exception(res.Errors.ToString());
        var entity = new CartEntity()
        {
            UserId = user.Id,
        };
        await appStoreContext.Carts.AddAsync(entity);
        await appStoreContext.SaveChangesAsync();

        if (!await roleManager.RoleExistsAsync("User"))
            await roleManager.CreateAsync(new RoleEntity("User"));
        await userManager.AddToRoleAsync(user, "User");

        var response = await jwtTokenService.CreateAuthResponse(user);
        return response;
    }

    public async Task LinkTelegram(LinkTelegramModel model)
    {
        var userId = await authService.GetUserIdAsync();
        var user = await userManager.FindByIdAsync(userId.ToString());
    
        user.TelegramChatId = model.ChatId;
        await userManager.UpdateAsync(user);
    }
}