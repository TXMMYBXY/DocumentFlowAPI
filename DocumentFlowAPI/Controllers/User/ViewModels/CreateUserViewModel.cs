namespace DocumentFlowAPI.Controllers.User.ViewModels;

/// <summary>
/// ViewModel для создания нового пользователя
/// </summary>
public class CreateUserViewModel
{
    public string Login { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
    public int DepartmentId { get; set; }
}