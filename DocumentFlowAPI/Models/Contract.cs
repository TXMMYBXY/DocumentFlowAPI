using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentFlowAPI.Models
{
    public class Contract : EntityBase
    {
        [Required]
        public string Title { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Content { get; set; }
        public DocumentStatus Status { get; set; }
        public int TemplateId { get; set; }
        
        [ForeignKey(nameof(TemplateId))]
        public virtual ContractTemplate Template { get; set; }
    }
    public enum DocumentStatus
    {
        Draft,
        Signed,
        Archived
    }
}