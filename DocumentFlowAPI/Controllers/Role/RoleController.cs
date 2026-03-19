using AutoMapper;
using DocumentFlowAPI.Controllers.Auth;
using DocumentFlowAPI.Controllers.Role.ViewModels;
using DocumentFlowAPI.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Controllers.Role;

[ApiController]
[Route("api/role")]
[AuthorizeByRoleId]
public class RoleController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRoleService _roleService;

    public RoleController(IMapper mapper, IRoleService roleService)
    {
        _mapper = mapper;
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<ActionResult<List<GetRoleViewModel>>> GetRoles()
    {
        var listRoleDto = await _roleService.GetAllRolesAsync();
        var listRoleViewModel = _mapper.Map<List<GetRoleViewModel>>(listRoleDto);

        return Ok(listRoleViewModel);
    }
}