using AutoMapper;
using DocumentFlowAPI.Controllers.Tasks.ViewModels;
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
    }
}
