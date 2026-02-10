using AutoMapper;
using DocumentFlowAPI.Controllers.Template.ViewModels;
using DocumentFlowAPI.Services.Template.Dto;
using DocumentFlowAPI.Services.WorkerTask.Dto;

namespace DocumentFlowAPI.Controllers.Template.MappingProfile;

public class TemplateMappingProfile : Profile
{
    public TemplateMappingProfile()
    {
        //Profiles for GET

        CreateMap<GetTemplateViewModel, GetTemplateDto>()
            .ReverseMap();

        CreateMap<Models.Template, GetTemplateDto>().ReverseMap();

        //Profiles for CREATE

        CreateMap<CreateTemplateViewModel, CreateTemplateDto>()
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ReverseMap();

        //Profiles for UPDATE

        CreateMap<UpdateTemplateViewModel, UpdateTemplateDto>().ReverseMap();

        //Profiles for Extract

        CreateMap<TemplateFieldInfoDto, TemplateFieldInfoViewModel>().ReverseMap();
    }
}
