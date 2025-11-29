using DocumentFlowAPI.Models;

namespace DocumentFlowAPI.Services.User.Dto;

public class GetUserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string? Email { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; }
    public virtual Role Role { get; set; }
    public virtual Department Department { get; set; }
}