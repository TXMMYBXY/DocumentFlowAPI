using DocumentFlowAPI.Models.AboutUserModels;
using DocumentFlowAPI.Services.Auth.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IJwtService
{
    /// <summary>
    /// Метод генерирует токен доступа как JWT
    /// </summary>
    /// <param name="user">Пользовательские данные которые идут в коллекцию клеймов</param>
    /// <returns>токен</returns>
    string GenerateAccessToken(Models.User user);

    /// <summary>
    /// Метод генерирует токен обновления
    /// </summary>
    /// <param name="userId">По id проверяет наличие токена дял пользователя</param>
    Task<RefreshToken> GenerateRefreshTokenAsync(int userId);

    /// <summary>
    /// Проверка на валидность для токена обновления
    /// </summary>
    Task<bool> ValidateRefreshTokenAsync(string refreshToken);
    
    /// <summary>
    /// Метод для получения id владельца токена
    /// </summary>
    Task<int> GetRefreshTokenOwnerAsync(string refreshToken);
}
