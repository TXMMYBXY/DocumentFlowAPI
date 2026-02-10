using DocumentFlowAPI.Base;
using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowAPI.Repositories;

public class TaskRepository : BaseRepository<TaskModel>, ITaskRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TaskRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TaskModel>> GetAllTasks()
    {
        return await _dbContext.Tasks.ToListAsync();
    }

    public async Task<TaskModel?> GetTaskByIdAsync(Guid taskId)
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);
    }

    public async Task<TaskModel?> GetTaskByStatusPendingAsync()
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Status == Models.TaskStatus.Pending);
    }
}
