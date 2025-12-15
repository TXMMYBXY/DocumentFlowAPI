using DocumentFlowAPI.Models.AboutUserModels;
using DocumentFlowAPI.Services.User.Dto;

namespace DocumentFlowAPI.Controllers.Auth.ViewModels;

public class AccessTokenResponseViewModel
{
    public UserInfoForLoginDto UserInfo { get; set; }
    public string AccessToken { get; set; }
    public string ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public RefreshToken RefreshToken { get; set; }
}
