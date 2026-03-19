using DocumentFlowAPI.Services.Role.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IRoleService
{
    Task<List<GetRoleDto>> GetAllRolesAsync();
}