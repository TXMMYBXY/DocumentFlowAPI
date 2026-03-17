using DocumentFlowAPI.Models;

namespace DocumentFlowAPI.Services.Personal.Dto;

public class PersonDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
}