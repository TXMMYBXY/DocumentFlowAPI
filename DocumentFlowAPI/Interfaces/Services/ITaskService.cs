using DocumentFlowAPI.Services.Tasks.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface ITaskService
{
    /// <summary>
    /// Создать задачу на генерацию документа
    /// </summary>
    Task<TaskResultDto> CreateTaskAsync(CreateTaskRequestDto dto);

    /// <summary>
    /// Получить задачу по Id (детально)
    /// </summary>
    Task<TaskDetailsDto?> GetTaskByIdAsync(Guid taskId);

    /// <summary>
    /// Получить список задач пользователя
    /// </summary>
    Task<List<TaskDetailsDto?>> GetAllTasksAsync(int userId);

    /// <summary>
    /// Отменить задачу (если ещё возможно)
    /// </summary>
    Task<bool> CancelTaskAsync(Guid taskId, TaskCancelDto dto);

    /// <summary>
    /// Повторно поставить задачу в очередь
    /// </summary>
    Task<bool> RetryTaskAsync(Guid taskId, int? userId);
}
