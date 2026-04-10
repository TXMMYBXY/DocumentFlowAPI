using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Repositories.Base;
using DocumentFlowAPI.Services.Department;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowAPI.Repositories;

public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Department>> GetAllDepartmentsAsync(DepartmentFilter filter)
    {
        var query = _dbContext.Departments
            .Include(d => d.Employees)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.Title)) query = query.Where(d => d.Title.Contains(filter.Title));
        if (filter.PageSize.HasValue && filter.PageNumber.HasValue)
        {
            query = query
                .Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value);
        }
        
        return await query
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _dbContext.Departments.CountAsync();
    }

    public async Task<bool> IsDepartmentHasEmployeesAsync(int departmentId)
    {
        return await _dbContext.Departments.Where(d => d.Id == departmentId)
            .Select(d => d.Employees.Any())
            .FirstOrDefaultAsync();
    }
}