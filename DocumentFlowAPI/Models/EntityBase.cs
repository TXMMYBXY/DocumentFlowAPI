using System.ComponentModel.DataAnnotations;

namespace DocumentFlowAPI.Models
{
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}