using AutoMapper;
using DocumentFlowAPI.Interfaces.Services;

namespace DocumentFlowAPI.Services.Tasks;

public class TaskService : ITaskService
{
    private readonly IMapper _mapper;

    public TaskService(IMapper mapper)
    {
        _mapper = mapper;
    }
}
