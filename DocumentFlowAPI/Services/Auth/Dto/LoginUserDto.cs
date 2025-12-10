namespace DocumentFlowAPI.Services.Auth.Dto;

public class LoginUserDto
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}
