using AutoMapper;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.WorkerTask.Dto;

namespace DocumentFlowAPI.Controllers.Worker.MappingProfile;

public class WorkerMappingProfile : Profile
{
    public WorkerMappingProfile()
    {
        CreateMap<WorkerTaskDto, TaskModel>().ReverseMap();
    }
}
