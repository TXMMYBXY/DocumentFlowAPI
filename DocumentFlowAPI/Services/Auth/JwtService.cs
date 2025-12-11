using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DocumentFlowAPI.Configuration;
using DocumentFlowAPI.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DocumentFlowAPI.Services.Auth;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    /// <summary>
    /// Генерация токена
    /// </summary>
    /// <param name="user"></param>
    /// <returns>токен</returns>
    public string GenerateToken(Models.User user)
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
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.ExpiresDays), // Время истечения
            SigningCredentials = new SigningCredentials(           // Подпись токена
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(jwtDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GetUserEmailFromToken(string token)
    {
        throw new NotImplementedException();
    }

    public int GetUserIdFromToken(string token)
    {
        throw new NotImplementedException();
    }

    public bool ValidateToken(string token)
    {
        throw new NotImplementedException();
    }
}
