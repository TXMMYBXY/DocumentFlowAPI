using AutoMapper;
using DocumentFlowAPI.Controllers.Template.ViewModels;
using DocumentFlowAPI.Services.Template.Dto;

namespace DocumentFlowAPI.Controllers.Template.MappingProfile;

public class TemplateMappingProfile : Profile
{
    public TemplateMappingProfile()
    {
        //Profiles for GET

        CreateMap<GetTemplateViewModel, GetTemplateDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Models.Template, GetTemplateDto>().ReverseMap();

        //Profiles for CREATE

        CreateMap<CreateTemplateViewModel, CreateTemplateDto>().ReverseMap();

        //Profiles for UPDATE

        CreateMap<UpdateTemplateViewModel, UpdateTemplateDto>().ReverseMap();
    }
}
