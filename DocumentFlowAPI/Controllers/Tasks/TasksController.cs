using AutoMapper;
using DocumentFlowAPI.Controllers.Tasks.ViewModels;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.Tasks.Dto;
using DocumentFlowAPI.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Controllers.Tasks;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITaskService _taskService;

    public TaskController(IMapper mapper, ITaskService taskService)
    {
        _mapper = mapper;
        _taskService = taskService;
    }

    /// <summary>
    /// Метод для создания задачи. Отправляет клиент
    /// </summary>
    /// <param name="taskViewModelRequest"></param>
    /// <returns></returns>
    [HttpPost("generate")]
    public async Task<ActionResult> CreateTask([FromBody] CreateTaskRequestViewModel taskViewModelRequest)
    {
        var taskDtoRequest = _mapper.Map<CreateTaskRequestDto>(taskViewModelRequest);
        var result = await _taskService.CreateTaskAsync(taskDtoRequest);
        var response = _mapper.Map<TaskResultViewModel>(result);

        return Accepted(response);
    }

    /// <summary>
    /// Метод для получения задачи по Id
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    [HttpGet("{taskId}/get-task")]
    public async Task<ActionResult<TaskDetailsViewModel>> GetTaskById([FromRoute] Guid taskId)
    {
        var taskDto = await _taskService.GetTaskByIdAsync(taskId);
        var responseViewModel = _mapper.Map<TaskDetailsViewModel>(taskDto);

        return Ok(responseViewModel);
    }

    /// <summary>
    /// Метод для получения всех задач пользователя
    /// </summary>
    /// <returns></returns>
    [HttpGet("get-all")]
    public async Task<ActionResult<List<TaskDetailsViewModel>>> GetTaskById()
    {
        var taskList = await _taskService.GetAllTasksAsync(UserIdentity.User!.Id);
        var responseViewModel = _mapper.Map<List<TaskDetailsViewModel>>(taskList);

        return Ok(responseViewModel);
    }

    /// <summary>
    /// Метод для отмены задачи по Id
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="taskCancelViewModel"></param>
    /// <returns></returns>
    [HttpPost("{taskId}/cancel")]
    public async Task<ActionResult> CancelTaskById([FromRoute] Guid taskId, [FromBody] TaskCancelViewModel taskCancelViewModel)
    {
        var taskDto = _mapper.Map<TaskCancelDto>(taskCancelViewModel);
        taskDto.UserId = UserIdentity.User!.Id;
        var result = await _taskService.CancelTaskAsync(taskId, taskDto);

        return Ok(result);
    }

    /// <summary>
    /// Метод для повтора задачи
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    [HttpPost("{taskId}/retry")]
    public async Task<ActionResult> RetryTaskByid([FromRoute] Guid taskId)
    {
        var success = await _taskService.RetryTaskAsync(taskId, UserIdentity.User!.Id);

        if (!success)
            return BadRequest("Задачу нельзя перезапустить");

        return Ok();
    }

}
