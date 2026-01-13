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
using System.Security.Cryptography;

namespace DocumentFlowAPI.Services.Auth;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly RefreshTokenSettings _refreshTokenSettings;
    private readonly ITokenRepository _tokenRepository;
    private readonly IRefreshTokenHasher _refreshTokenHashser;

    public JwtService(
        IOptions<JwtSettings> jwtSettings,
        IOptions<RefreshTokenSettings> refreshTokenSettings,
        ITokenRepository tokenRepository,
        IRefreshTokenHasher refreshTokenHasher)
    {
        _jwtSettings = jwtSettings.Value;
        _refreshTokenSettings = refreshTokenSettings.Value;
        _tokenRepository = tokenRepository;
        _refreshTokenHashser = refreshTokenHasher;
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
            new Claim("Department", user.Department.ToString()),
            new Claim("RoleId", user.RoleId.ToString()),
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

        var secretKey = _GenerateSecretLine();
        var refreshToken = new RefreshToken
        {
            Token = _refreshTokenHashser.Hash(secretKey),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenSettings.ExpiresDays)//FIXME:AddMinutes -> AddDays
        };

        await _tokenRepository.CreateRefreshTokenAsync(refreshToken);
        await _tokenRepository.SaveChangesAsync();

        refreshToken.Token = secretKey;

        return refreshToken;
    }

    public async Task<bool> ValidateRefreshTokenAsync(RefreshToken refreshToken)
    {
        var token = await _tokenRepository.GetRefreshTokenByUserIdAsync(refreshToken.UserId);

        return token.Token.Equals(_refreshTokenHashser.Hash(refreshToken.Token));
    }

    /// <summary>
    /// Метод для генерации посследовательности
    /// </summary>
    private string _GenerateSecretLine()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
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

        return token.Token.Equals(_refreshTokenHashser.Hash(accessTokenDto.RefreshToken));
    }
}
