namespace DocumentFlowAPI.Controllers.User.ViewModels;

/// <summary>
/// ViewModel для обновления информации о пользователе
/// </summary>
public class UpdateUserViewModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
    public int RoleId { get; set; }
}