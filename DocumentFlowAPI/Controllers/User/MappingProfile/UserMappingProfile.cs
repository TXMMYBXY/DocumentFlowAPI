using AutoMapper;
using DocumentFlowAPI.Controllers.User.ViewModels;
using DocumentFlowAPI.Services.User.Dto;

namespace DocumentFlowAPI.Controllers.User.MappingProfile;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<NewUserViewModel, NewUserDto>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
            .ReverseMap();

        CreateMap<NewUserViewModel, NewUserDto>().ReverseMap();
    }
}
