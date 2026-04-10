using AutoMapper;
using DocumentFlowAPI.Controllers.Auth;
using DocumentFlowAPI.Controllers.Department.ViewModels;
using DocumentFlowAPI.Enums;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.Department;
using DocumentFlowAPI.Services.Department.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Controllers.Department;

[ApiController]
[Route("api/department")]
[AuthorizeByRoleId((int)Permissions.Admin)]
public class DepartmentController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IMapper mapper, IDepartmentService departmentService)
    {
        _mapper = mapper;
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedDepartmentViewModel>> GetAllDepartments([FromQuery] DepartmentFilter filter)
    {
        var pagedDepartmentDto = await _departmentService.GetAllDepartmentsAsync(filter);
        var pagedDepartmentViewModel = _mapper.Map<PagedDepartmentViewModel>(pagedDepartmentDto);
        
        return Ok(pagedDepartmentViewModel);
    }

    [HttpGet("{id:int}/department-info")]
    public async Task<ActionResult<GetDepartmentViewModel>> GetDepartmentInfo([FromRoute] int id)
    {
        var departmentDto = await _departmentService.GetDepartmentByIdAsync(id);
        var departmentViewModel = _mapper.Map<GetDepartmentViewModel>(departmentDto);
        
        return Ok(departmentViewModel);
    }

    [HttpPost]
    public async Task<ActionResult> CreateDepartment([FromBody] CreateDepartmentViewModel createDepartmentViewModel)
    {
        var createDepartmentDto = _mapper.Map<CreateDepartmentDto>(createDepartmentViewModel);

        await _departmentService.CreateDepartmentAsync(createDepartmentDto);

        return Created();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateDepartment([FromRoute] int id,
        [FromBody] UpdateDepartmentViewModel updateDepartmentViewModel)
    {
        var updateDepartmentDto = _mapper.Map<UpdateDepartmentDto>(updateDepartmentViewModel);
        
        await _departmentService.UpdateDepartmentAsync(id, updateDepartmentDto);
        
        return Ok();
    }

    [HttpDelete("{departmentId:int}")]
    public async Task<ActionResult> DeleteDepartment([FromRoute] int departmentId)
    {
        await _departmentService.DeleteDepartmentAsync(departmentId);
        
        return Ok();
    }
}