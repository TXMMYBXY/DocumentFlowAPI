using AutoMapper;
using DocumentFlowAPI.Controllers.Role.ViewModels;
using DocumentFlowAPI.Services.Role.Dto;

namespace DocumentFlowAPI.Controllers.Role.MappingProfile;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        //Profiles for GET
        CreateMap<GetRoleDto, GetRoleViewModel>();

        CreateMap<Models.Role, GetRoleDto>();
    }
}