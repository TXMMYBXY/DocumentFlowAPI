using DocumentFlowAPI.Services.User.Dto;

namespace DocumentFlowAPI.Controllers.Auth.ViewModels;

public class RegisterResponseViewModel
{
    public UserInfoDto UserInfo { get; set; }
    public string Token { get; set; }
    public string ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
}
