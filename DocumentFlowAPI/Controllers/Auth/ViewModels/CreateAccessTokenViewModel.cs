using System.Text.Json.Serialization;

namespace DocumentFlowAPI.Controllers.Auth.ViewModels;

public class CreateAccessTokenViewModel
{
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
}
