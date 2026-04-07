using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentFlowAPI.Models.AboutUserModels;

public class RefreshToken : EntityBase
{
    [MaxLength(127)]
    public string? Token { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }
}
