namespace DocumentFlowAPI.Controllers.Auth.ViewModels;

public class RegisterRequestViewModel
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public int RoleId { get; set; }
    public int DepartmentId { get; set; }
}
