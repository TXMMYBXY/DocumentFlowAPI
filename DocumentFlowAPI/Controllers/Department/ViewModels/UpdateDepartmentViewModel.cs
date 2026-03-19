using System.ComponentModel.DataAnnotations;

namespace DocumentFlowAPI.Controllers.Department.ViewModels;

public class UpdateDepartmentViewModel
{
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    public List<int>? EmployeesIds { get; set; }
}