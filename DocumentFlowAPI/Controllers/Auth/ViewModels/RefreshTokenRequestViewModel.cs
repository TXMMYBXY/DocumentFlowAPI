using DocumentFlowAPI.Services.User;

namespace DocumentFlowAPI.Controllers.Auth.ViewModels;

public class RefreshTokenRequestViewModel
{
    public int UserId { get; set; }
    public string Token { get; set; }
}
