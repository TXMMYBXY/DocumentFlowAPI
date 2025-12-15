using DocumentFlowAPI.Base;
using DocumentFlowAPI.Models.AboutUserModels;
using DocumentFlowAPI.Services.Auth.Dto;

namespace DocumentFlowAPI.Interfaces.Repositories;

public interface ITokenRepository : IBaseRepository<RefreshToken>
{
    /// <summary>
    /// Добавляет токен обновления в таблицу
    /// </summary>
    Task CreateRefreshTokenAsync(RefreshToken refreshToken);

    /// <summary>
    /// Возвращает токен обновления из таблицы по userId
    /// </summary>
    Task<RefreshToken?> GetRefreshTokenByUserIdAsync(int userId);

    /// <summary>
    /// Удаляет токен обновления из таблицы
    /// </summary>
    void DeleteRefreshToken(RefreshToken refreshToken);
}
