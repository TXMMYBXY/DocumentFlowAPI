namespace DocumentFlowAPI.Controllers.Department.ViewModels;

public class UpdateDepartmentViewModel
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public List<int>? EmployeesIds { get; set; }
}