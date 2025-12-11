using System.ComponentModel.DataAnnotations;

namespace DocumentFlowAPI.Models;

public class Role : EntityBase
{
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
}

public enum Permissions
{
    Admin = 1,
    Boss = 2,
    Purchaser = 3,
    Staff = 4
}