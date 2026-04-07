using DocumentFlowAPI.Services.Auth.Dto;

namespace DocumentFlowAPI.Controllers.Auth.ViewModels;

public class RefreshTokenToLoginResponseViewModel
{
    public bool IsAllowed { get; set; }
    public RefreshTokenDto RefreshToken { get; set; }
}
