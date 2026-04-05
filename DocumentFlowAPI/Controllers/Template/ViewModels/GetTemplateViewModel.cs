using System.Text.Json.Serialization;
using DocumentFlowAPI.Controllers.User.ViewModels;

namespace DocumentFlowAPI.Controllers.Template.ViewModels;

public class GetTemplateViewModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("createdBy")]
    public int CreatedBy { get; set; }

    [JsonPropertyName("user")]
    public virtual GetUserViewModel User { get; set; }
    
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
}
