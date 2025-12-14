using DocumentFlowAPI.Base;
using DocumentFlowAPI.Models.AboutUserModels;
using DocumentFlowAPI.Services.Auth.Dto;

namespace DocumentFlowAPI.Interfaces.Repositories;

public interface ITokenRepository : IBaseRepository<RefreshToken>
{
    Task CreateRefreshTokenAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetRefreshTokenByUserIdAsync(int userId);
    RefreshToken UpdateRefreshToken(RefreshToken refreshToken);
    void DeleteRefreshToken(RefreshToken refreshToken);
    Task<bool> ValidateRefreshTokenAsync(RefreshToken refreshToken);
}
