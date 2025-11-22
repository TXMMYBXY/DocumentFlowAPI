using DocumentFlowAPI.Models;

namespace DocumentFlowAPI.Services.User.Dto;

public class UserInfoDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
    public virtual Role Role { get; set; }
    public int DepartmentId { get; set; }
    public virtual Department Department { get; set; }
}
