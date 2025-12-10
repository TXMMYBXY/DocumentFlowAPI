namespace DocumentFlowAPI.Controllers.User.ViewModels;

/// <summary>
/// ViewModel для создания нового пользователя
/// </summary>
public class CreateUserViewModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public int RoleId { get; set; }
    public int DepartmentId { get; set; }
}