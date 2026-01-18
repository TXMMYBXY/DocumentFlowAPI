using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.Tasks.Dto;

namespace DocumentFlowAPI.Services.Tasks;

public class TaskService : ITaskService
{
    private readonly IMapper _mapper;
    private readonly ITaskRepository _taskRepository;

    public TaskService(IMapper mapper, ITaskRepository taskRepository)
    {
        _mapper = mapper;
        _taskRepository = taskRepository;
    }

    public async Task<bool> CancelTaskAsync(Guid taskId, TaskCancelDto dto)
    {
        var task = await _taskRepository.GetTaskByIdAsync(BitConverter.ToInt32(taskId.ToByteArray()));

        if (task == null)
            return false;

        // Проверка пользователя (если используется)
        if (task.UserId != dto.UserId)
            return false;

        // Можно отменять только Pending
        if (task.Status != Models.TaskStatus.Pending)
            return false;

        task.Status = Models.TaskStatus.Failed;
        task.ErrorMessage = string.IsNullOrWhiteSpace(dto.Reason)
            ? "Отменено пользователем"
            : dto.Reason;

        task.UpdatedAt = DateTime.UtcNow;
        task.CompletedAt = DateTime.UtcNow;

        await _taskRepository.SaveChangesAsync();
        return true;
    }

    public Task<TaskResultDto> CreateTaskAsync(CreateTaskRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskDetailsDto?> GetTaskByIdAsync(Guid taskId)
    {
        var task = await _taskRepository.GetTaskByIdAsync(BitConverter.ToInt32(taskId.ToByteArray()));
        var taskDto = _mapper.Map<TaskDetailsDto>(task);

        return taskDto;
    }

    public async Task<bool> RetryTaskAsync(Guid taskId, int? userId)
    {
        var task = await _taskRepository.GetTaskByIdAsync(BitConverter.ToInt32(taskId.ToByteArray()));

        if (task == null)
            return false;

        if (userId.HasValue && task.UserId != userId)
            return false;

        if (task.Status != Models.TaskStatus.Failed)
            return false;

        // 3. Сброс состояния
        task.Status = Models.TaskStatus.Pending;
        task.ErrorMessage = null;
        task.ResultFilePath = null;
        task.StartedAt = null;
        task.CompletedAt = null;
        task.UpdatedAt = DateTime.UtcNow;

        // 4. (опционально) повысить приоритет
        task.Priority = TaskPriority.Normal;

        await _taskRepository.SaveChangesAsync();
        return true;
    }

    public async Task<List<TaskDetailsDto?>> GetAllTasksAsync(int userId)
    {
        var taskList = await _taskRepository.GetAllTasks();
        var taskListDto = _mapper.Map<List<TaskDetailsDto>>(taskList);

        return taskListDto;
    }
}
