using DocumentFlowAPI.Services.User.Dto;

namespace DocumentFlowAPI.Services.Auth.Dto;

public class LoginResponseDto
{
    public UserInfoForLoginDto UserInfo { get; set; }
    public string AccessToken { get; set; }
    public string ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public string RefreshToken { get; set; }
}
