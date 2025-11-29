using DocumentFlowAPI.Services.User.Dto;

namespace DocumentFlowAPI.Services.Auth.Dto;

public class LoginResponseDto
{
    public UserInfoForLoginDto UserInfo { get; set; }
    public string Token { get; set; }
    public string ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
}
