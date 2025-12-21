using DocumentFlowAPI.Models.AboutUserModels;

namespace DocumentFlowAPI.Controllers.Auth.ViewModels;

public class RefreshTokenToLoginResponseViewModel
{
    public bool IsAllowed { get; set; }
    public RefreshToken RefreshToken { get; set; }
}
