using System.ComponentModel.DataAnnotations;

namespace DocumentFlowAPI.Models;

public class Department : EntityBase
{
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<User>? Employees { get; set; }
}