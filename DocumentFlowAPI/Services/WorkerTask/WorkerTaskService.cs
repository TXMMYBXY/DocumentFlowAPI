using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.WorkerTask.Dto;

namespace DocumentFlowAPI.Services.WorkerTask;

public class WorkerTaskService : IWorkerTaskService
{
    public Task CompleteAsyncById(Guid taskId, WorkerTaskCompletedDto dto)
    {
        throw new NotImplementedException();
    }

    public Task FailAsyncById(Guid taskId, WorkerTaskFailedDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<WorkerTaskDto?> GetNextAsync(Guid workerId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProgressAsync(Guid taskId, WorkerTaskProgressDto dto)
    {
        throw new NotImplementedException();
    }
}
