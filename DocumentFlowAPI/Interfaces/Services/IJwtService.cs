using DocumentFlowAPI.Models.AboutUserModels;
using DocumentFlowAPI.Services.Auth.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IJwtService
{
    string GenerateAccessToken(Models.User user);
    Task<RefreshToken> GenerateRefreshTokenAsync(int userId);
    Task<bool> ValidateRefreshTokenAsync(RefreshToken refreshToken);
    void RefreshTokenValue(RefreshToken refreshToken);
}
