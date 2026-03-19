using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentFlowAPI.Models;

public class LoginHistory : EntityBase
{
    [Required]
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }
    
    [Column(TypeName = "datetime")]
    public DateTime? LoginDate { get; set; } = DateTime.UtcNow;
}