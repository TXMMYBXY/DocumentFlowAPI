using DocumentFlowAPI.Models.AboutUserModels;

namespace DocumentFlowAPI.Services.Auth.Dto;

public class RefreshTokenToLoginResponseDto
{
    public bool IsAllowed { get; set; } = false;
    public RefreshToken RefreshToken { get; set; } = null;
}
