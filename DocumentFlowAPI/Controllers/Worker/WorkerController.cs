using AutoMapper;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.WorkerTask.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Controllers.Worker;

[ApiController]
[Route("api/internal/tasks/worker")]
[WorkerAuthorize]
public class WorkerController : ControllerBase
{
    public readonly IMapper _mapper;
    public readonly IWorkerTaskService _workerTaskService;

    public WorkerController(IMapper mapper, IWorkerTaskService workerTaskService)
    {
        _mapper = mapper;
        _workerTaskService = workerTaskService;
    }

    /// <summary>
    /// Метод для получения и блокировки следующей задачи
    /// </summary>
    [HttpPost("next")]
    public async Task<ActionResult<WorkerTaskDto>> GetNextTask()
    {
        var workerId = Guid.NewGuid();

        var task = await _workerTaskService.GetNextAsync(workerId);
        if (task == null)
            return NoContent();

        return Ok(task);
    }

    /// <summary>
    /// Метод для завершения задачи успешно
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="workerTaskCompleteDto"></param>
    /// <returns></returns>
    [HttpPost("{taskId}/complete")]
    public async Task<ActionResult> CompleteTaskById([FromRoute] Guid taskId, [FromBody] WorkerTaskCompletedDto workerTaskCompleteDto)
    {
        await _workerTaskService.CompleteAsyncById(taskId, workerTaskCompleteDto);

        return Ok();
    }

    /// <summary>
    /// Метод для завершения задачи неудачно
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="workerTaskFailedDto"></param>
    /// <returns></returns>
    [HttpPost("{taskId}/fail")]
    public async Task<ActionResult> FailTaskById([FromRoute] Guid taskId, [FromBody] WorkerTaskFailedDto workerTaskFailedDto)
    {
        await _workerTaskService.FailAsyncById(taskId, workerTaskFailedDto);

        return Ok();
    }

    /// <summary>
    /// Метод для обновления прогресса выполнения задачи
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="workerTaskProgressDto"></param>
    /// <returns></returns>
    [HttpPost("{taskId}/progress")]
    public async Task<ActionResult> ProgressTaskById([FromRoute] Guid taskId, [FromBody] WorkerTaskProgressDto workerTaskProgressDto)
    {
        await _workerTaskService.UpdateProgressAsync(taskId, workerTaskProgressDto);

        return Ok();
    }
    
}
