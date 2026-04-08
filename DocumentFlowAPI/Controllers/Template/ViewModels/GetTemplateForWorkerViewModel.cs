using System.Text.Json.Serialization;

namespace DocumentFlowAPI.Controllers.Template.ViewModels;

public class GetTemplateForWorkerViewModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("path")]
    public string FilePath { get; set; } = string.Empty;
}
