using AutoMapper;
using DocumentFlowAPI.Controllers.Tasks.ViewModels;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.Tasks.Dto;

namespace DocumentFlowAPI.Controllers.Tasks.MappingProfile;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        CreateMap<CreateTaskRequestViewModel, CreateTaskRequestDto>().ReverseMap();

        CreateMap<TaskCancelViewModel, TaskCancelDto>()
            .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<CreateTaskRequestDto, TaskModel>()
            .ForMember(dest => dest.TaskId, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => Models.TaskStatus.Pending))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            
        CreateMap<TaskResultViewModel, TaskResultDto>().ReverseMap();
    }
}
