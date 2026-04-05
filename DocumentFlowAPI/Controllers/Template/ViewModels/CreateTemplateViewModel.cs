using System.Text.Json.Serialization;

namespace DocumentFlowAPI.Controllers.Template.ViewModels;

public class CreateTemplateViewModel
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    [JsonPropertyName("file")]
    public IFormFile File { get; set; } = null!;
}
