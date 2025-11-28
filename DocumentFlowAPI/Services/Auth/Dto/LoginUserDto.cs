namespace DocumentFlowAPI.Services.Auth.Dto;

public class LoginUserDto
{
    public string Login { get; set; }
    public string PasswordHash { get; set; }
}
