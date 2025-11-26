using AutoMapper;
using DocumentFlowAPI.Controllers.Template.ViewModels;
using DocumentFlowAPI.Services.Template.Dto;

namespace DocumentFlowAPI.Controllers.Template.MappingProfile;

public class TemplateMappingProfile : Profile
{
    public TemplateMappingProfile()
    {
        CreateMap<TemplateDto, TemplateViewModel>().ReverseMap();

        CreateMap<NewTemplateViewModel, NewTemplateDto>().ReverseMap();

        CreateMap<UpdateTemplateViewModel, UpdateTemplateDto>().ReverseMap();

        CreateMap<Models.Template, TemplateDto>().ReverseMap();
        
        CreateMap<GetTemplateViewModel, TemplateDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ReverseMap();
    }
}
