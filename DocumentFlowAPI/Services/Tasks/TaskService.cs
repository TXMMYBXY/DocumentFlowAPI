using System.Text.Json;
using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.General;
using DocumentFlowAPI.Services.Tasks.Dto;

namespace DocumentFlowAPI.Services.Tasks;

public class TaskService : GeneralService, ITaskService
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
        var task = await _taskRepository.GetTaskByIdAsync(taskId);

        if (task == null || 
            task.UserId != dto.UserId ||
            task.Status != Models.TaskStatus.Pending)
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

    public async Task<TaskResultDto> CreateTaskAsync(CreateTaskRequestDto dto)
    {
        var templateDataJson = JsonSerializer.Serialize(dto.Data);

        var task = _mapper.Map<TaskModel>(dto);

        task.TemplateData = templateDataJson;

        await _taskRepository.AddAsync(task);
        await _taskRepository.SaveChangesAsync();

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
        var task = await _taskRepository.GetTaskByIdAsync(taskId);

        if (task == null)
            return false;

        if (userId.HasValue && task.UserId != userId)
            return false;

        if (task.Status != Models.TaskStatus.Failed)
            return false;

        _RetryTaskFillFields(task);

        await _taskRepository.SaveChangesAsync();
        return true;
    }

    public async Task<List<TaskDetailsDto?>> GetAllTasksAsync(int userId)
    {
        var taskList = await _taskRepository.GetAllTasks();
        var taskListDto = _mapper.Map<List<TaskDetailsDto>>(taskList);

        return taskListDto;
    }

    private void _RetryTaskFillFields(TaskModel task)
    {
        task.Status = Models.TaskStatus.Pending;
        task.ErrorMessage = null;
        task.ResultFilePath = null;
        task.StartedAt = null;
        task.CompletedAt = null;
        task.UpdatedAt = DateTime.UtcNow;
        task.Priority = TaskPriority.Normal;
    }
}
