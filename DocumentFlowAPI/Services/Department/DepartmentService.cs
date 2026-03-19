using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Repositories.Users;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.Department.Dto;
using DocumentFlowAPI.Services.General;

namespace DocumentFlowAPI.Services.Department;

public class DepartmentService : IDepartmentService
{
    private readonly IMapper _mapper;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUserRepository _userRepository;

    public DepartmentService(
        IMapper mapper, 
        IDepartmentRepository departmentRepository,
        IUserRepository userRepository)
    {
        _mapper = mapper;
        _departmentRepository = departmentRepository;
        _userRepository = userRepository;
    }
    
    public async Task<List<GetDepartmentDto>> GetAllDepartmentsAsync(DepartmentFilter filter)
    {
        var departments = await _departmentRepository.GetAllDepartmentsAsync(filter);
        var listDepartmentDto = _mapper.Map<List<GetDepartmentDto>>(departments);
        
        return listDepartmentDto;
    }

    public async Task<GetDepartmentDto> GetDepartmentByIdAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        var departmentDto = _mapper.Map<GetDepartmentDto>(department);
        
        return departmentDto;
    }

    public async Task CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto)
    {
        var department = _mapper.Map<Models.Department>(createDepartmentDto);
        
        await _departmentRepository.AddAsync(department);
        await _departmentRepository.SaveChangesAsync();
    }

    public async Task UpdateDepartmentAsync(int departmentId, UpdateDepartmentDto updateDepartmentDto)
    {
        var department = await _departmentRepository.GetByIdAsync(departmentId);
        
        GeneralService.NullCheck(department, "Department is not exists");

        _mapper.Map(updateDepartmentDto, department);

        foreach (var employeeId in updateDepartmentDto.EmployeesIds)
        {
            var employee = await _userRepository.GetByIdAsync(employeeId);

            if (employee == null) continue;
            
            employee.DepartmentId = departmentId;
            
            _userRepository.UpdateFields(employee, user => user.DepartmentId);
        }
        
        await _userRepository.SaveChangesAsync();
        await _departmentRepository.SaveChangesAsync();
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        
        GeneralService.NullCheck(department, "Department is not exists");

        if (department.Employees.Count != 0)
        {
            throw new InvalidOperationException("Department has employees");
        }
        
        _departmentRepository.Delete(department);
        
        await _departmentRepository.SaveChangesAsync();
    }
}