using DocumentFlowAPI.Interfaces.Repositories.Users.Dtos;

namespace DocumentFlowAPI.Services.Department.Dto;

public class GetDepartmentDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public List<EmployeeDto>? Employees { get; set; }
}