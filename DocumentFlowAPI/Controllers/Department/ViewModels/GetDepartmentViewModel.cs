using DocumentFlowAPI.Services.Department.Dto;

namespace DocumentFlowAPI.Controllers.Department.ViewModels;

public class GetDepartmentViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public virtual List<EmployeeDto> Employees { get; set; }
}