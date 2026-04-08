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
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(
        IMapper mapper,
        IDepartmentRepository departmentRepository,
        IUserRepository userRepository,
        ILogger<DepartmentService> logger)
    {
        _mapper = mapper;
        _departmentRepository = departmentRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<PagedDepartmentDto> GetAllDepartmentsAsync(DepartmentFilter filter)
    {
        var departments = await _departmentRepository.GetAllDepartmentsAsync(filter);
        var listDepartmentDto = _mapper.Map<List<GetDepartmentDto>>(departments);

        return new PagedDepartmentDto
        {
            Departments = listDepartmentDto,
            TotalCount = await _departmentRepository.GetTotalCountAsync(),
            PageSize = filter.PageSize ?? listDepartmentDto.Count,
            CurrentPage = filter.PageNumber ?? 1
        };
    }

    public async Task<GetDepartmentDto> GetDepartmentByIdAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        var departmentDto = _mapper.Map<GetDepartmentDto>(department);

        return departmentDto;
    }

    public async Task CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto)
    {
        _logger.LogInformation("Creating department with name: {Title}", createDepartmentDto.Title);

        var department = _mapper.Map<Models.Department>(createDepartmentDto);

        await _departmentRepository.AddAsync(department);
        await _departmentRepository.SaveChangesAsync();

        _logger.LogInformation("Department created with id: {Id}", department.Id);
    }

    public async Task UpdateDepartmentAsync(int departmentId, UpdateDepartmentDto updateDepartmentDto)
    {
        _logger.LogInformation("Updating department with id: {Id}", departmentId);
        
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

        _logger.LogInformation("Department with id: {Id} updated successfully", departmentId);
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        _logger.LogInformation("Attempting to delete department with id: {Id}", id);
        
        var isDepartmentHasEmployees = await _departmentRepository.IsDepartmentHasEmployeesAsync(id);

        GeneralService.NullCheck(isDepartmentHasEmployees, "Department is not exists");

        if (isDepartmentHasEmployees)
        {
            throw new InvalidOperationException("Department has employees");
        }

        await _departmentRepository.DeleteAsync(id);
        await _departmentRepository.SaveChangesAsync();

        _logger.LogInformation("Department with id: {Id} deleted successfully", id);
    }
}