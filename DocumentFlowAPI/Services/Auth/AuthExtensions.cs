using System.Text;
using DocumentFlowAPI.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DocumentFlowAPI.Services.Auth;

/// <summary>
/// Класс расширений для авторизации и аутентификации
/// </summary>
public static class AuthExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        var authSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

        serviceCollection
        .AddAuthorization()
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;  // Для разработки (в продакшене должно быть true)
            options.SaveToken = true;              // Сохраняем токен в контексте
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,                        // Проверяем подпись
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(authSettings.SecretKey)),   // Ключ для проверки
                ValidateIssuer = true,                                  // Проверяем издателя
                ValidIssuer = authSettings.Issuer,                      // Владидный издатель
                ValidateAudience = true,                                // Проверяем аудиторию
                ValidAudience = authSettings.Audience,                  // Валидная аудитория
                ValidateLifetime = true,                                // Проверяем время жизни
                ClockSkew = TimeSpan.Zero                               // Не даем дополнительного времени
            };
        });

        return serviceCollection;
    }
}
