using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentFlowAPI.Models;

public abstract class Template : EntityBase
{
    public string Title { get; set; }
    public string Path { get; set; }
    [ForeignKey(nameof(CreatedBy))]
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }
}