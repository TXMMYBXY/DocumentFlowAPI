using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentFlowAPI.Models;

public abstract class Document : EntityBase
{
    [Required]
    public string Title { get; set; }
    public string Path { get; set; }
    [ForeignKey(nameof(Id))]
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
}