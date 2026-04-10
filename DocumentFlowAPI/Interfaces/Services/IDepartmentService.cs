using DocumentFlowAPI.Services.Department;
using DocumentFlowAPI.Services.Department.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IDepartmentService
{
    Task<PagedDepartmentDto> GetAllDepartmentsAsync(DepartmentFilter filter);
    Task<GetDepartmentDto> GetDepartmentByIdAsync(int id);
    Task CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto);
    Task UpdateDepartmentAsync(int departmetnId, UpdateDepartmentDto updateDepartmentDto);
    Task DeleteDepartmentAsync(int id);
}