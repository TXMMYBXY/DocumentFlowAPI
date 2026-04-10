using System.Text.Json.Serialization;
using DocumentFlowAPI.Services.General;

namespace DocumentFlowAPI.Controllers.Template.ViewModels;

public class PagedTemplateViewModel : PagedData
{
    [JsonPropertyName("templates")]
    public List<GetTemplateViewModel> Templates { get; set; }
}
