using AutoMapper;
using DocumentFlowAPI.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Controllers.Tasks;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITaskService _taskService;

    public TasksController(IMapper mapper, ITaskService taskService)
    {
        _mapper = mapper;
        _taskService = taskService;
    }
    
    // [HttpPost("generate")]
    // public async Task<ActionResult> CreateGenerationTask([FromBody] CreateGenerationTaskViewModelRequest taskViewModelRequest)
    // {
        
        
    //     return Accepted();
    // }

}
