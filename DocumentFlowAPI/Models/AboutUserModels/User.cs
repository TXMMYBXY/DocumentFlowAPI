using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentFlowAPI.Models;

public class User : EntityBase
{
    [Required]
    public string FullName { get; set; }
    [EmailAddress]
    [MaxLength(63)]
    public string Email { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; } = true;
    public int RoleId { get; set; }
    [ForeignKey(nameof(RoleId))]
    public virtual Role Role { get; set; }
    public int DepartmentId { get; set; }
    [ForeignKey(nameof(DepartmentId))]
    public virtual Department Department { get; set; }
}