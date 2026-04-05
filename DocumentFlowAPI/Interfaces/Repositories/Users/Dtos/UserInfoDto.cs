namespace DocumentFlowAPI.Interfaces.Repositories.Users.Dtos;

public class UserInfoDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Department { get; set; }
    public string Role { get; set; }
}
