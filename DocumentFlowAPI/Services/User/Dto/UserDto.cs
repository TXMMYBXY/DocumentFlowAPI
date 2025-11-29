using DocumentFlowAPI.Models;

namespace DocumentFlowAPI.Services.User.Dto;

public class UserDto
{
    public int Id { get; set;}
    public string FullName { get; set; }
    public string? Email { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; } = true;
    public int RoleId { get; set; }
    public int DepartmentId { get; set; }
}