using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.Role.Dto;

namespace DocumentFlowAPI.Services.Role;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly IRoleRepository _roleRepository;

    public RoleService(IMapper mapper, IRoleRepository roleRepository)
    {
        _mapper = mapper;
        _roleRepository = roleRepository;
    }
    
    public async Task<List<GetRoleDto>> GetAllRolesAsync()
    {
        var listRole = await _roleRepository.GetAllAsync();
        var listRoleDto = _mapper.Map<List<GetRoleDto>>(listRole);
        
        return listRoleDto;
    }
}