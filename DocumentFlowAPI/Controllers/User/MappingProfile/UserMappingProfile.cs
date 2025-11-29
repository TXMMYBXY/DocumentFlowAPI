using AutoMapper;
using DocumentFlowAPI.Controllers.User.ViewModels;
using DocumentFlowAPI.Services.User.Dto;

namespace DocumentFlowAPI.Controllers.User.MappingProfile;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        //Profiles for GET

        CreateMap<Models.User, GetUserDto>().ReverseMap();

        CreateMap<GetUserDto, GetUserViewModel>().ReverseMap();

        //Profiles for CREATE

        CreateMap<CreateUserViewModel, CreateUserDto>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
            .ReverseMap();

        CreateMap<CreateUserDto, Models.User>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => ""))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => ""));

        //Profiles for UPDATE

        CreateMap<UpdateUserViewModel, UpdateUserDto>().ReverseMap();

        CreateMap<UpdateUserDto, Models.User>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<ResetPasswordViewModel, ResetPasswordDto>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
    }
}
