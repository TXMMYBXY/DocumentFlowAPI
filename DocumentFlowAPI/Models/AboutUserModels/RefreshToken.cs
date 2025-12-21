using System.ComponentModel.DataAnnotations;

namespace DocumentFlowAPI.Models.AboutUserModels;

public class RefreshToken : EntityBase
{
    [MaxLength(31)]
    public string? Token { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int UserId { get; set; }
}
