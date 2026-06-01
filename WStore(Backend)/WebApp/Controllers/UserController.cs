using Core.Interfaces;
using Core.Models.User;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controlers;
[Route("api/[controller]/[action]")]
[ApiController]
public class UserController
    (UserManager<UserEntity> userManager, IUserService userService,
    IJwtTokenService jwtTokenService, IMinioImageService imageService,
    RoleManager<RoleEntity> roleManager, IGoogleAuthService googleAuthService,
    IAuthService authService
    ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized("Invalid email or password.");

        var response = await jwtTokenService.CreateAuthResponse(user);
        return Ok(response);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Register([FromForm] RegisterModel model)
    {
        var existingUser = await userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
            return BadRequest("User already exists.");

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
            return BadRequest(res.Errors);

        if (!await roleManager.RoleExistsAsync("User"))
            await roleManager.CreateAsync(new RoleEntity("User"));
        await userManager.AddToRoleAsync(user, "User");

        var response = await jwtTokenService.CreateAuthResponse(user);
        return Ok(response);
    }

    [HttpPost]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> EditProfile([FromForm] EditProfileModel model)
    {
        var response = await userService.EditProfileAsync(model);
        return Ok(response); 
    }
    
    [HttpPost]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestModel model)
    {
        try
        {
            var response = await jwtTokenService.RefreshTokenAsync(model.RefreshToken);
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid or expired refresh token");
        }
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        var user = await userManager.FindByEmailAsync(email!);
        if (user != null)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await userManager.UpdateAsync(user);
        }
        return Ok();
    }
    [HttpPost]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestModel model)
    {
        var result = await googleAuthService.LoginByGoogleAsync(model.IdToken);
        if (result == null)
            return BadRequest(new { Status = 400, Errors = new { Email = "Помилка реєстрації" } });

        return Ok(result); 
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
    {
        bool res = await userService.ForgotPasswordAsync(model);
        if (res)
        {
            return Ok();
        }
        else
        {
            return BadRequest(new
            {
                Status = 400,
                IsValid = false,
                Errors = new { Email = "Користувача з такою поштою не існує" }
            });
        } 
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        var isTry = await userService.ResetPasswordAsync(model);
        if (!isTry)
        {
            return BadRequest(new
            {
                Status = 400,
                IsValid = false,
                Errors = new { Email = "Невірні дані для відновлення паролю" }
            });
        }
        return Ok();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        var user = await userService.GetUserProfileAsync();
        return Ok(user);
    }
    [HttpPost("link-telegram")]
    [Authorize]
    public async Task<IActionResult> LinkTelegram([FromBody] LinkTelegramModel model)
    {
        var userId = await authService.GetUserIdAsync();
        var user = await userManager.FindByIdAsync(userId.ToString());
    
        user.TelegramChatId = model.ChatId;
        await userManager.UpdateAsync(user);
    
        return Ok();
    }
    
    
        
}