using AutoMapper;
using DocumentFlowAPI.Controllers.Auth.ViewModels;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Models.AboutUserModels;
using DocumentFlowAPI.Services.Auth.Dto;
using DocumentFlowAPI.Services.Personal.Dto;
using DocumentFlowAPI.Services.User.Dto;

namespace DocumentFlowAPI.Controllers.Auth.MappingProfile;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        //Profiles for login

        CreateMap<LoginUserDto, LoginRequestViewModel>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
            .ReverseMap();

        CreateMap<LoginResponseDto, LoginResponseViewModel>()
            .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AccessToken))
            .ForMember(dest => dest.ExpiresAt, opt => opt.MapFrom(src => src.ExpiresAt))
            .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserInfo))
            .ForMember(dest => dest.TokenType, opt => opt.MapFrom(src => src.TokenType))
            .ReverseMap();

        CreateMap<Models.User, UserInfoForLoginDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.Title));


        CreateMap<RefreshTokenToLoginDto, RefreshTokenToLoginViewModel>()
            .ReverseMap();

        CreateMap<RefreshTokenToLoginResponseViewModel, RefreshTokenToLoginResponseDto>()
            .ReverseMap();

        CreateMap<NewAuthRecordDto, LoginHistory>();

        //profiles for refresh

        CreateMap<RefreshTokenRequestDto, RefreshTokenRequestViewModel>()
            .ReverseMap();

        CreateMap<RefreshTokenResponseViewModel, RefreshTokenResponseDto>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<RefreshToken, RefreshTokenRequestDto>()
            .ReverseMap();

        CreateMap<RefreshTokenResponseDto, RefreshToken>()
            .ReverseMap();

        CreateMap<RefreshToken, RefreshTokenDto>()
            .ReverseMap();

        //profiles for access

        CreateMap<AccessTokenDto, AccessTokenViewModel>()
            .ReverseMap();

        CreateMap<AccessTokenResponseDto, AccessTokenResponseViewModel>()
            .ReverseMap();
    }
}
