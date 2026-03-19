using System.Text.Json.Serialization;
using DocumentFlowAPI.Services.Department.Dto;

namespace DocumentFlowAPI.Controllers.Department.ViewModels;

public class GetDepartmentViewModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("employees")]
    public virtual List<EmployeeDto> Employees { get; set; }
}