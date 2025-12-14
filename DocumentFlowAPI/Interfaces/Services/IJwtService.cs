using DocumentFlowAPI.Models.AboutUserModels;
using DocumentFlowAPI.Services.Auth.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IJwtService
{
    string GenerateAccessToken(Models.User user);
    string GenerateRefreshToken(int userId);
    Task<bool> ValidateRefreshToken(RefreshToken refreshToken);
    void RefreshTokenValue(RefreshToken refreshToken); 
}
