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

    public async Task<RefreshToken> GenerateRefreshTokenAsync(int userId)
    {
        var targetToken = await _tokenRepository.GetRefreshTokenByUserIdAsync(userId);
        if (targetToken != null)
        {
            _RevokeToken(targetToken);
        }
        //BUG: Здесь при установке времени в 1 минуту клиентское приложение падает
        var refreshToken = new RefreshToken
        {
            Token = _GenerateSecret(),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.ExpiresDays)//FIXME:AddMinutes -> AddDays
        };

        await _tokenRepository.CreateRefreshTokenAsync(refreshToken);
        await _tokenRepository.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<bool> ValidateRefreshTokenAsync(RefreshToken refreshToken)
    {
        var token = await _tokenRepository.GetRefreshTokenByUserIdAsync(refreshToken.UserId);

        return token.Token == refreshToken.Token;
    }

    /// <summary>
    /// Метод для генерации посследовательности
    /// </summary>
    private string _GenerateSecret()
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

    /// <summary>
    /// Метод для удаления токена из таблицы
    /// </summary>
    /// <param name="refreshToken"></param>
    private void _RevokeToken(RefreshToken refreshToken)
    {
        _tokenRepository.DeleteRefreshToken(refreshToken);
    }

    public async Task<bool> ValidateAccessTokenAsync(AccessTokenDto accessTokenDto)
    {
        var token = await _tokenRepository.GetRefreshTokenByUserIdAsync(accessTokenDto.UserId);

        return token.Token == accessTokenDto.RefreshToken;
    }
}
