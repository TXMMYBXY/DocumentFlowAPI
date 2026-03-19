using DocumentFlowAPI.Models;

namespace DocumentFlowAPI.Services.User;

public class UserFilter
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public int? DepartmentId { get; set; }
    public int? RoleId { get; set; }
    
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
}
