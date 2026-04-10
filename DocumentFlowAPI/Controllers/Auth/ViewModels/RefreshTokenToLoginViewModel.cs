using System.Text.Json.Serialization;

namespace DocumentFlowAPI.Controllers.Auth.ViewModels;

public class RefreshTokenToLoginViewModel
{
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
}
