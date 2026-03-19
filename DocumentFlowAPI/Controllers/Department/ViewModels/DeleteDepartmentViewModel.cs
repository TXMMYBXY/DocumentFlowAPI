using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentFlowAPI.Controllers.Department.ViewModels;

public class DeleteDepartmentViewModel
{
    [Required]
    [JsonPropertyName("departmentId")]
    public int DepartmentId { get; set; }
}