namespace DocumentFlowAPI.Controllers.Auth.ViewModels;

public class RefreshTokenResponseViewModel
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}
