using System.Text.Json.Serialization;

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
    public virtual Models.User User { get; set; }
    
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
}
