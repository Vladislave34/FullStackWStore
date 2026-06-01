using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Core.Interfaces;
using Core.Models.User;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SixLabors.ImageSharp;

namespace Core.Services;

public class JwtTokenService(IConfiguration configuration, UserManager<UserEntity> userManager) : IJwtTokenService
{
    public async Task<string> CreateToken(UserEntity user)
    {
        var key = configuration["Jwt:Key"];
        var claims = new List<Claim>
        {
            new Claim("email", user.Email ?? "do not have an email"),
            new Claim("username", user.UserName ?? "do not have an username"),
            new Claim("image", user.Image),
            new Claim("firstName", user.FirstName ),
            new Claim("lastName", user.LastName ),
        };
        foreach (var role in await userManager.GetRolesAsync(user))
        {
            claims.Add(new Claim("roles", role));
        }
        var keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
        
        var symmetricSecurityKey = new SymmetricSecurityKey(keyBytes);

        var signingCredentials = new SigningCredentials(
            symmetricSecurityKey,
            SecurityAlgorithms.HmacSha256
        );

        var jwtSecurityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: signingCredentials
        );
        string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        return token;
    }
    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
    public async Task<AuthResponseModel> CreateAuthResponse(UserEntity user)
    {
        var accessToken = await CreateToken(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);

        return new AuthResponseModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponseModel> RefreshTokenAsync(string refreshToken)
    {
        var user = await userManager.Users
            .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

        if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid or expired refresh token");

        return await CreateAuthResponse(user);
    }
}

