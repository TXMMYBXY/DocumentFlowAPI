using System.Text.Json.Serialization;

namespace DocumentFlowAPI.Services.Template.Dto;

public class GetTemplateForWorkerDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("path")]
    public string FilePath { get; set; } = string.Empty;
}
