using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentFlowAPI.Models;

public class Contract : EntityBase
{
    [Required]
    public string Title { get; set; }
    public string Path { get; set; }
    [ForeignKey(nameof(Id))]
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
    public int TemplateId { get; set; }
    
    [ForeignKey(nameof(ContractTemplate.Id))]
    public virtual ContractTemplate Template { get; set; }
}
public enum DocumentStatus
{
    Draft,
    Signed,
    Archived
}
