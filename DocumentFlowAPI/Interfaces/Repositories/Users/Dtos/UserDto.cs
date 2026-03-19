using DocumentFlowAPI.Models;

namespace DocumentFlowAPI.Interfaces.Repositories.Users.Dtos;

public class UserDto
{
    public int  Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public Department Department { get; set; }
    public Role Role { get; set; }
}