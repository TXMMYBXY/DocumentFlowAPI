using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentFlowAPI.Models
{
    public class Contract : EntityBase
    {
        public int TemplateId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Content { get; set; }
        public DocumentStatus Status { get; set; }
    }
    public enum DocumentStatus
    {
        Draft,
        Signed,
        Archived
    }
}