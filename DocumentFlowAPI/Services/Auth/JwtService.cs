using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DocumentFlowAPI.Configuration;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Models.AboutUserModels;
using DocumentFlowAPI.Services.Auth.Dto;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DocumentFlowAPI.Services.Auth;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ITokenRepository _tokenRepository;

    public JwtService(IOptions<JwtSettings> jwtSettings, ITokenRepository tokenRepository)
    {
        _jwtSettings = jwtSettings.Value;
        _tokenRepository = tokenRepository;
    }

    /// <summary>
    /// Генерация токена
    /// </summary>
    /// <param name="user"></param>
    /// <returns>токен</returns>
    public string GenerateAccessToken(Models.User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        //Коллекция данных которые будут храниться в токене
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.RoleId.ToString()),
            new Claim("RoleId", user.RoleId.ToString()),
            new Claim("DepartmentId", user.DepartmentId.ToString()),
            new Claim("IsActive", user.IsActive.ToString())
        };

        var jwtDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),                    // Утверждения (данные пользователя)
            Issuer = _jwtSettings.Issuer,                            // Издатель токена
            Audience = _jwtSettings.Audience,                        // Потребитель токена
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresMinutes), // Время истечения
            SigningCredentials = new SigningCredentials(           // Подпись токена
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(jwtDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken(int userId)
    {
        var refreshToken = new RefreshToken
        {
            Token = _GenerateSecret(),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.ExpiresDays)
        };

        _tokenRepository.CreateRefreshTokenAsync(refreshToken);

        return refreshToken.Token;
    }
    private static string _GenerateSecret()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var length = 31;
        var secret = new char[length];
        for (var i = 0; i < length; i++)
        {
            secret[i] = chars[random.Next(chars.Length)];
        }
        return new string(secret);
    }

    public async Task<bool> ValidateRefreshToken(RefreshToken refreshToken)
    {
        return await _tokenRepository.ValidateRefreshTokenAsync(refreshToken);
    }

    public void RefreshTokenValue(RefreshToken refreshToken)
    {
        refreshToken.Token = _GenerateSecret();
        refreshToken.ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.ExpiresDays);
        
        _tokenRepository.UpdateRefreshToken(refreshToken);
    }
}
