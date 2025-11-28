using DocumentFlowAPI.Services.User.Dto;

namespace DocumentFlowAPI.Services.Auth.Dto;

public class RegisterResponseDto
{
    public string Token { get; set; }
    public UserInfoDto UserInfo { get; set; }
    public string ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
}
