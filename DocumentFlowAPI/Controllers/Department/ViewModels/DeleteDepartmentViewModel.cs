using System.ComponentModel.DataAnnotations;

namespace DocumentFlowAPI.Controllers.Department.ViewModels;

public class DeleteDepartmentViewModel
{
    [Required]
    public int DepartmentId { get; set; }
}