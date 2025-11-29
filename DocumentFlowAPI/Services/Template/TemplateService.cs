using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.Template.Dto;

namespace DocumentFlowAPI.Services.Template;

public class TemplateService : ITemplateService
{
    private readonly IMapper _mapper;
    private readonly ITemplateRepository _templateRepository;
    public TemplateService(IMapper mapper, ITemplateRepository templateRepository)
    {
        _mapper = mapper;
        _templateRepository = templateRepository;
    }

    public async Task CreateTemplateAsync<T>(CreateTemplateDto templateDto) where T : Models.Template, new()
    {
        T templateModel = new T
        {
            Title = templateDto.Title,
            Path = templateDto.Path,
            CreatedBy = templateDto.CreatedBy,
            CreatedAt = templateDto.CreatedAt,
            IsActive = templateDto.IsActive
        };

        await _templateRepository.CreateTemplateAsync<T>(templateModel);
        await _templateRepository.SaveChangesAsync();
    }

    public async Task DeleteTemplateAsync<T>(int templateId) where T : Models.Template
    {
        var template = await _templateRepository.GetTemplateByIdAsync<T>(templateId);

        template.IsActive = false;

        _templateRepository.UpdateTemplateStatus<T>(template);

        await _templateRepository.SaveChangesAsync();
    }

    public async Task<List<GetTemplateDto>> GetAllTemplatesAsync<T>() where T : Models.Template
    {
        var templates = await _templateRepository.GetAllTemplatesAsync<T>();

        return _mapper.Map<List<GetTemplateDto>>(templates);
    }

    public async Task<GetTemplateDto> GetTemplateByIdAsync<T>(int templateId) where T : Models.Template
    {
        var temaplate = await _templateRepository.GetTemplateByIdAsync<T>(templateId);

        return _mapper.Map<GetTemplateDto>(temaplate);
    }

    public async Task UpdateTemplateAsync<T>(int templateId, UpdateTemplateDto templateDto) where T : Models.Template, new()
    {
        var template = await _templateRepository.GetTemplateByIdAsync<T>(templateId);
        var templateModel = _UpdateTemplate(template, templateDto);

        _templateRepository.UpdateTemplate<T>(templateModel);

        await _templateRepository.SaveChangesAsync();
    }

    private T _UpdateTemplate<T>(T template, UpdateTemplateDto templateDto) where T : Models.Template
    {
        template.Title = templateDto.Title;
        template.Path = templateDto.Path;

        return template;
    }
}