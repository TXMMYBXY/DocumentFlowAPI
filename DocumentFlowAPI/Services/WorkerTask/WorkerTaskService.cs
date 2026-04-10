using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.WorkerTask.Dto;

namespace DocumentFlowAPI.Services.WorkerTask;

public class WorkerTaskService : IWorkerTaskService
{
    private readonly IMapper _mapper;
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<WorkerTaskService> _logger;

    public WorkerTaskService(
        IMapper mapper,
        ITaskRepository taskRepository,
        ILogger<WorkerTaskService> logger)
    {
        _mapper = mapper;
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task CompleteAsyncById(Guid taskId, WorkerTaskCompletedDto dto)
    {
        _logger.LogInformation("Completing task {TaskId} with result file {ResultFilePath}", taskId, dto.ResultFilePath);

        var task = await _taskRepository.GetTaskByIdAsync(taskId);
        
        if (task == null)
            throw new KeyNotFoundException($"Task {taskId} not found");

        task.Status = Models.TaskStatus.Completed;
        task.ResultFilePath = dto.ResultFilePath;
        task.CompletedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        _taskRepository.Update(task);

        await _taskRepository.SaveChangesAsync();

        _logger.LogInformation("Task {TaskId} completed successfully", taskId);
    }

    public async Task FailAsyncById(Guid taskId, WorkerTaskFailedDto dto)
    {
        _logger.LogInformation("Failing task {TaskId} with error message: {ErrorMessage}", taskId, dto.ErrorMessage);
        
        var task = await _taskRepository.GetTaskByIdAsync(taskId);

        if (task == null)
            throw new KeyNotFoundException($"Task {taskId} not found");

        task.Status = Models.TaskStatus.Failed;
        task.ErrorMessage = dto.ErrorMessage;
        task.CompletedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        _taskRepository.Update(task);

        await _taskRepository.SaveChangesAsync();

        _logger.LogInformation("Task {TaskId} failed with error: {ErrorMessage}", taskId, dto.ErrorMessage);
    }

    public async Task<WorkerTaskDto?> GetNextAsync(Guid workerId)
    {
        _logger.LogInformation("Worker {WorkerId} is requesting the next task", workerId);
        
        var task = await _taskRepository.GetTaskByStatusPendingAsync();

        if (task == null)
            return null;

        var workerTaskDto = _mapper.Map<WorkerTaskDto>(task);

        _logger.LogInformation("Assigned task {TaskId} to worker {WorkerId}", task.Id, workerId);

        return workerTaskDto;
    }

    public async Task UpdateProgressAsync(Guid taskId, WorkerTaskProgressDto dto)
    {
        _logger.LogInformation("Updating progress for task {TaskId} to {Progress}", taskId, dto.Progress);
        
        var task = await _taskRepository.GetTaskByIdAsync(taskId);
        if (task == null)
            throw new KeyNotFoundException($"Task {taskId} not found");

        // Можно хранить прогресс в числовом поле (например, RetryCount или добавить Progress)
        task.UpdatedAt = DateTime.UtcNow;

        // Пример: временно используем RetryCount как прогресс
        task.RetryCount = dto.Progress;

        _taskRepository.Update(task);

        await _taskRepository.SaveChangesAsync();
        
        _logger.LogInformation("Progress for task {TaskId} updated to {Progress}", taskId, dto.Progress);
    }
}
