using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.Role.Dto;

namespace DocumentFlowAPI.Services.Department.Dto;

public class EmployeeDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public RoleDto Role { get; set; }
}