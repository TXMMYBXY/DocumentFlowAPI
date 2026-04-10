using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using TaskStatus = DocumentFlowAPI.Enums.TaskStatus;

namespace DocumentFlowAPI.Repositories;

public class TaskRepository : BaseRepository<TaskModel>, ITaskRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TaskRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TaskModel?> GetTaskByIdAsync(Guid taskId)
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);
    }

    public async Task<TaskModel?> GetTaskByStatusPendingAsync()
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Status == TaskStatus.Pending);
    }
}
