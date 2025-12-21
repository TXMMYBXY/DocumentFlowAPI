namespace DocumentFlowAPI.Services.Auth.Dto;

public class AccessTokenDto
{
    public int UserId { get; set; }
    public string RefreshToken { get; set; }
}
