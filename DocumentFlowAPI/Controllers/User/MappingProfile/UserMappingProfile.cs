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

        CreateMap<Models.User, UserDto>().ReverseMap();

        CreateMap<Models.User, UserInfoDto>().ReverseMap();

        CreateMap<UserInfoViewModel, UserInfoDto>().ReverseMap();

        CreateMap<UpdateUserInfoViewModel, UpdateUserInfoDto>().ReverseMap();

        CreateMap<UpdateUserInfoDto, Models.User>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
            .ReverseMap();

        CreateMap<UserInfoDto, UserInfoViewModel>().ReverseMap();

        CreateMap<NewUserDto, Models.User>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => ""))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => ""));

        CreateMap<UserInfoDto, Models.User>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
            .ReverseMap();
    }
}
