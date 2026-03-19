using DocumentFlowAPI.Models;

namespace DocumentFlowAPI.Controllers.User.ViewModels;

/// <summary>
/// ViewModel со всей информацией о пользователе
/// </summary>
public class GetUserViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public string Department { get; set; }
    public virtual Models.Role Role { get; set; }
}