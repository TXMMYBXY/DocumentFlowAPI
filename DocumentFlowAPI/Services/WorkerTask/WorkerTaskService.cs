using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.WorkerTask.Dto;

namespace DocumentFlowAPI.Services.WorkerTask;

public class WorkerTaskService : IWorkerTaskService
{
    private readonly IMapper _mapper;
    private readonly ITaskRepository _taskRepository;

    public WorkerTaskService(IMapper mapper, ITaskRepository taskRepository)
    {
        _mapper = mapper;
        _taskRepository = taskRepository;
    }

    public async Task CompleteAsyncById(Guid taskId, WorkerTaskCompletedDto dto)
    {
        var task = await _taskRepository.GetTaskByIdAsync(taskId);
        if (task == null)
            throw new KeyNotFoundException($"Task {taskId} not found");

        task.Status = Models.TaskStatus.Completed;
        task.ResultFilePath = dto.ResultFilePath;
        task.CompletedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        _taskRepository.Update(task);
        await _taskRepository.SaveChangesAsync();
    }

    public async Task FailAsyncById(Guid taskId, WorkerTaskFailedDto dto)
    {
        var task = await _taskRepository.GetTaskByIdAsync(taskId);
        if (task == null)
            throw new KeyNotFoundException($"Task {taskId} not found");

        task.Status = Models.TaskStatus.Failed;
        task.ErrorMessage = dto.ErrorMessage;
        task.CompletedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        _taskRepository.Update(task);
        await _taskRepository.SaveChangesAsync();
    }

    public async Task<WorkerTaskDto?> GetNextAsync(Guid workerId)
    {
        var task = await _taskRepository.GetTaskByStatusPendingAsync();

        if (task == null)
            return null;

        var workerTaskDto = _mapper.Map<WorkerTaskDto>(task);

        return workerTaskDto;
    }

    public async Task UpdateProgressAsync(Guid taskId, WorkerTaskProgressDto dto)
    {
        var task = await _taskRepository.GetTaskByIdAsync(taskId);
        if (task == null)
            throw new KeyNotFoundException($"Task {taskId} not found");

        // Можно хранить прогресс в числовом поле (например, RetryCount или добавить Progress)
        task.UpdatedAt = DateTime.UtcNow;

        // Пример: временно используем RetryCount как прогресс
        task.RetryCount = dto.Progress;

        _taskRepository.Update(task);
        
        await _taskRepository.SaveChangesAsync();
    }
}
