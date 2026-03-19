using DocumentFlowAPI.Models;

namespace DocumentFlowAPI.Services.Personal.Dto;

public class GetPersonDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
    public Models.Role Role { get; set; }
}