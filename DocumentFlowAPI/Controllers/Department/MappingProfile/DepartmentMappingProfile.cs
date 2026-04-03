using AutoMapper;
using DocumentFlowAPI.Controllers.Department.ViewModels;
using DocumentFlowAPI.Services.Department.Dto;

namespace DocumentFlowAPI.Controllers.Department.MappingProfile;

public class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        //Profiles for GET        
        CreateMap<GetDepartmentDto, GetDepartmentViewModel>().ReverseMap();

        CreateMap<PagedDepartmentDto, PagedDepartmentViewModel>().ReverseMap();

        CreateMap<Models.Department, GetDepartmentDto>()
            .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.Employees))
            .ReverseMap();

        CreateMap<Models.User, EmployeeDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

        CreateMap<Models.Role, RoleDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            
        
        //Profiles for POST
        CreateMap<CreateDepartmentViewModel, CreateDepartmentDto>();

        CreateMap<CreateDepartmentDto, Models.Department>();
        
        //Profiles for PUT
        CreateMap<UpdateDepartmentViewModel, UpdateDepartmentDto>();
        
        CreateMap<UpdateDepartmentDto, Models.Department>();
    }
}