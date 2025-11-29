namespace DocumentFlowAPI.Services.Auth.Dto;

public class RegisterUserDto
{
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public int RoleId { get; set; }
    public int DepartmentId { get; set; }
}
