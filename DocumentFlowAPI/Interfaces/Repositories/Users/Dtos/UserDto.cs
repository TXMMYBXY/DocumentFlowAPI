namespace DocumentFlowAPI.Interfaces.Repositories.Users.Dtos;

public class UserDto
{
    public int  Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
    public int RoleId { get; set; }
}