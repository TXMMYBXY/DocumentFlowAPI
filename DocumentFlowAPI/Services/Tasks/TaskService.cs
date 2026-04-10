using System.Text.Json;
using AutoMapper;
using DocumentFlowAPI.Enums;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.General;
using DocumentFlowAPI.Services.Tasks.Dto;
using TaskStatus = DocumentFlowAPI.Enums.TaskStatus;

namespace DocumentFlowAPI.Services.Tasks;

public class TaskService : GeneralService, ITaskService
{
    private readonly IMapper _mapper;
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<TaskService> _logger;

    public TaskService(
        IMapper mapper,
        ITaskRepository taskRepository,
        ILogger<TaskService> logger)
    {
        _mapper = mapper;
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task<bool> CancelTaskAsync(Guid taskId, TaskCancelDto dto)
    {
        _logger.LogInformation("User with id: {UserId} tried to canceled task with id: {TaskId}", dto.UserId, taskId);
        
        var task = await _taskRepository.GetTaskByIdAsync(taskId);

        if (task == null || 
            task.UserId != dto.UserId ||
            task.Status != TaskStatus.Pending)
            return false;

        task.Status = TaskStatus.Failed;
        task.ErrorMessage = string.IsNullOrWhiteSpace(dto.Reason)
            ? "Отменено пользователем"
            : dto.Reason;

        task.UpdatedAt = DateTime.UtcNow;
        task.CompletedAt = DateTime.UtcNow;

        await _taskRepository.SaveChangesAsync();

        _logger.LogInformation("User with id: {UserId} successfully canceled task with id: {TaskId}", dto.UserId, taskId);

        return true;
    }

    public async Task<TaskResultDto> CreateTaskAsync(CreateTaskRequestDto dto)
    {
        _logger.LogInformation("User with id: {UserId} is creating a task of template type: {TemplateType}", dto.UserId, dto.TemplateType);
        
        var templateDataJson = JsonSerializer.Serialize(dto.Data);

        var task = _mapper.Map<TaskModel>(dto);

        task.TemplateData = templateDataJson;

        await _taskRepository.AddAsync(task);
        await _taskRepository.SaveChangesAsync();

        _logger.LogInformation("User with id: {UserId} successfully created a task with id: {TaskId} of type: {TemplateType}",
        dto.UserId, task.TaskId, dto.TemplateType);

        return new TaskResultDto
        {
            TaskId = task.TaskId,
            Status = task.Status,
            Message = "Задача успешно создана"
        };
    }

    public async Task<TaskDetailsDto?> GetTaskByIdAsync(Guid taskId)
    {
        var task = await _taskRepository.GetTaskByIdAsync(taskId);
        var taskDto = _mapper.Map<TaskDetailsDto>(task);

        return taskDto;
    }

    public async Task<bool> RetryTaskAsync(Guid taskId, int? userId)
    {
        _logger.LogInformation("User with id: {UserId} is trying to retry task with id: {TaskId}", userId, taskId);
        
        var task = await _taskRepository.GetTaskByIdAsync(taskId);

        if (task == null)
            return false;

        if (userId.HasValue && task.UserId != userId)
            return false;

        if (task.Status != TaskStatus.Failed)
            return false;

        _RetryTaskFillFields(task);

        await _taskRepository.SaveChangesAsync();

        _logger.LogInformation("User with id: {UserId} successfully retried task with id: {TaskId}", userId, taskId);

        return true;
    }

    public async Task<List<TaskDetailsDto?>> GetAllTasksAsync(int userId)
    {
        var taskList = await _taskRepository.GetAllAsync();
        var taskListDto = _mapper.Map<List<TaskDetailsDto>>(taskList);

        return taskListDto;
    }

    private void _RetryTaskFillFields(TaskModel task)
    {
        task.Status = TaskStatus.Pending;
        task.ErrorMessage = null;
        task.ResultFilePath = null;
        task.StartedAt = null;
        task.CompletedAt = null;
        task.UpdatedAt = DateTime.UtcNow;
        task.Priority = TaskPriority.Normal;
    }
}
