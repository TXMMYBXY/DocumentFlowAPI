namespace DocumentFlowAPI.Services.Department.Dto;

public class UpdateDepartmentDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public List<int>? EmployeesIds { get; set; }
}