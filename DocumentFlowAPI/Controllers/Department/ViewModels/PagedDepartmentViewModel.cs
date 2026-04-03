using System.Text.Json.Serialization;
using DocumentFlowAPI.Services.General;

namespace DocumentFlowAPI.Controllers.Department.ViewModels;

public class PagedDepartmentViewModel : PagedData
{
    [JsonPropertyName("departments")]
    public List<GetDepartmentViewModel> Departments { get; set; }
}
