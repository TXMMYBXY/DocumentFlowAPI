using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentFlowAPI.Models;

public class Contract : Document
{
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
