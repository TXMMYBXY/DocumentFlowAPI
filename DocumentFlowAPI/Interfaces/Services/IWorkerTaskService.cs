using DocumentFlowAPI.Services.WorkerTask.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IWorkerTaskService
{
    /// <summary>
    /// Получить и атомарно заблокировать следующую задачу
    /// </summary>
    Task<WorkerTaskDto?> GetNextAsync(Guid workerId);

    /// <summary>
    /// Обновить прогресс выполнения
    /// </summary>
    Task UpdateProgressAsync(Guid taskId, WorkerTaskProgressDto dto);

    /// <summary>
    /// Пометить задачу как успешно завершённую
    /// </summary>
    Task CompleteAsyncById(Guid taskId, WorkerTaskCompletedDto dto);

    /// <summary>
    /// Пометить задачу как ошибочную
    /// </summary>
    Task FailAsyncById(Guid taskId, WorkerTaskFailedDto dto);
}
