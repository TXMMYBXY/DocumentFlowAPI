using System.Text.Json.Serialization;
using DocumentFlowAPI.Models;

namespace DocumentFlowAPI.Controllers.PersonalAccount.ViewModels;

public class GetPersonViewModel
{
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("department")]
    public string Department { get; set; }
    
    [JsonPropertyName("role")]
    public Models.Role Role { get; set; }
}