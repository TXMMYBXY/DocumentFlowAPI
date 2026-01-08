using DocumentFlowAPI.Models.AboutUserModels;

namespace DocumentFlowAPI.Services.Auth.Dto;

public class RefreshTokenToLoginResponseDto
{
    public bool IsAllowed { get; set; } = false;
}
