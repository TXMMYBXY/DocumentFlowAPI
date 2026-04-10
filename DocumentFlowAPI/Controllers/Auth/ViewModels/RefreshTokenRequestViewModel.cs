using System.Text.Json.Serialization;
using DocumentFlowAPI.Services.User;

namespace DocumentFlowAPI.Controllers.Auth.ViewModels;

public class RefreshTokenRequestViewModel
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
}
