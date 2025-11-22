namespace DocumentFlowAPI.Controllers.User.ViewModels;

public class UpdateUserInfoViewModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
    public int DepartmentId { get; set; }
}