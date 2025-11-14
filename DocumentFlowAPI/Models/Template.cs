using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentFlowAPI.Models
{
    public abstract class Template : EntityBase
    {
        public string Title { get; set; }
        public string Content { get; set; }
        [ForeignKey(nameof(Id))]
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }
    }
}