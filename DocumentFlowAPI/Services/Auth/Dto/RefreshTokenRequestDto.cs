namespace DocumentFlowAPI.Services.Auth.Dto;

public class RefreshTokenRequestDto
{
    public int UserId { get; set; }
    public string Token { get; set; }
}
