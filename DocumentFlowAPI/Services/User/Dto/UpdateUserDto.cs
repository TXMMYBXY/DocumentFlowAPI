namespace DocumentFlowAPI.Services.User.Dto;

public class UpdateUserDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
    public int RoleId { get; set; }
    public int DepartmentId { get; set; }
}
