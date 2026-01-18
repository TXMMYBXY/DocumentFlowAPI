using DocumentFlowAPI.Base;
using DocumentFlowAPI.Models;

namespace DocumentFlowAPI.Interfaces.Repositories;

public interface ITaskRepository : IBaseRepository<TaskModel>
{
    Task<TaskModel?> GetTaskByIdAsync(Guid taskId);
    Task<List<TaskModel>> GetAllTasks();
}
