using System.Text.Json.Serialization;

namespace DocumentFlowAPI.Controllers.Template.ViewModels;

public class DeleteManyTemplatesViewModel
{
    [JsonPropertyName("templateIds")]
    public List<int> TemplateIds { get; set; } = new();
}
