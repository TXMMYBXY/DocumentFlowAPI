using DocumentFlowAPI.Interfaces.Base;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.Department;

namespace DocumentFlowAPI.Interfaces.Repositories;

public interface IDepartmentRepository : IBaseRepository<Department>
{
    Task<List<Department>> GetAllDepartmentsAsync(DepartmentFilter filter);
}