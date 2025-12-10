namespace DocumentFlowAPI.Services.User.Dto;

public class CreateUserDto
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FullName { get; set; }
    public int RoleId { get; set; }
    public int DepartmentId { get; set; }
}
