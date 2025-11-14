using System.ComponentModel.DataAnnotations;

namespace DocumentFlowAPI.Models;

public class Role : EntityBase
{
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
}