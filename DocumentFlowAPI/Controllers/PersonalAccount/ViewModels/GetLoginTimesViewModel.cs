using System.Text.Json.Serialization;
using DocumentFlowAPI.Services.Personal.Dto;

namespace DocumentFlowAPI.Controllers.PersonalAccount.ViewModels;

public class GetLoginTimesViewModel
{
    [JsonPropertyName("loginTime")]
    public DateTime? LoginTime { get; set; }
}