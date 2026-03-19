using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentFlowAPI.Controllers.PersonalAccount.ViewModels;

public class ChangePasswordViewModel
{
    [Required]
    [JsonPropertyName("currentPassword")]
    public string CurrentPassword { get; set; }
    
    [Required]
    [JsonPropertyName("newPassword")]
    public string NewPassword { get; set; }
    
    [Required]
    [JsonPropertyName("confirmPassword")]
    public string ConfirmPassword { get; set; }
}