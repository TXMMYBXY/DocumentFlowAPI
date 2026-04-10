using System.Text.Json.Serialization;

namespace DocumentFlowAPI.Controllers.Template.ViewModels;

public class DownloadTemplateViewModel
{
    [JsonPropertyName("filePath")]
    public string FilePath { get; set; }

    [JsonPropertyName("fileName")]
    public string FileName { get; set; }
}
