namespace DocumentFlowAPI.Services.Auth.Dto;

public class RefreshTokenDto
{
    public int UserId { get; set; }
    public string Token { get; set; }
}
