using System.ComponentModel.DataAnnotations;

namespace DocumentFlowAPI.Controllers.User.ViewModels;

/// <summary>
/// ViewModel для создания нового пользователя
/// </summary>
public class CreateUserViewModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string FullName { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public int RoleId { get; set; }
}