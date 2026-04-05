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

        CreateMap<GetTemplateViewModel, GetTemplateDto>().ReverseMap();

        CreateMap<Models.Template, GetTemplateDto>().ReverseMap();

        CreateMap<PagedTemplateViewModel, PagedTemplateDto>().ReverseMap();

        CreateMap<DownloadTemplateDto, DownloadTemplateViewModel>().ReverseMap();

        //Profiles for CREATE

        CreateMap<CreateTemplateViewModel, CreateTemplateDto>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.File.FileName))
            .ForMember(dest => dest.FileLength, opt => opt.MapFrom(src => src.File.Length));

        //Profiles for UPDATE

        CreateMap<UpdateTemplateViewModel, UpdateTemplateDto>().ReverseMap();

        //Profiles for Extract

        CreateMap<TemplateFieldInfoDto, TemplateFieldInfoViewModel>().ReverseMap();
    }
}
