using System.ComponentModel.DataAnnotations;

namespace DocumentFlowAPI.Controllers.Department.ViewModels;

public class CreateDepartmentViewModel
{
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
}