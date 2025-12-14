namespace DocumentFlowAPI.Services.Auth.Dto;

public class RefreshTokenDto
{
    public string? Token { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int? UserId { get; set; }
}
