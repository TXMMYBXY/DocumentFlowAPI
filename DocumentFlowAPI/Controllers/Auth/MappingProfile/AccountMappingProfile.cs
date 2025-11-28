using AutoMapper;
using DocumentFlowAPI.Controllers.Auth.ViewModels;
using DocumentFlowAPI.Services.Auth.Dto;

namespace DocumentFlowAPI.Controllers.Auth.MappingProfile;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<RegisterUserViewModel, RegisterUserDto>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
            .ReverseMap();

        CreateMap<LoginUserDto, LoginUserViewModel>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
            .ReverseMap();

        CreateMap<RegisterUserDto, Models.User>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => ""))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => ""));

        CreateMap<LoginResponseDto, LoginResponseViewModel>()
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token))
            .ForMember(dest => dest.ExpiresAt, opt => opt.MapFrom(src => src.ExpiresAt))
            .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserInfo))
            .ForMember(dest => dest.TokenType, opt => opt.MapFrom(src => src.TokenType))
            .ReverseMap();

        CreateMap<RegisterResponseDto, RegisterResponseViewModel>().ReverseMap();
        
    }
}
