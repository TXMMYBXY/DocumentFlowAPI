using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentFlowAPI.Models
{
    public class Token : EntityBase
    {
        public string TokenValue { get; set; }
        public DateTime PayAtLast { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}