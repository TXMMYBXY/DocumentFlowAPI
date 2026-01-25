using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.Template.Dto;
using DocumentFlowAPI.Services.WorkerTask.Dto;

namespace DocumentFlowAPI.Services.Template;

public class TemplateService : ITemplateService
{
    private readonly IMapper _mapper;
    private readonly ITemplateRepository _templateRepository;
    private readonly IFieldExtractorService _fieldExtractorService;

    public TemplateService(
        IMapper mapper,
        ITemplateRepository templateRepository,
        IFieldExtractorService fieldExtractorService)
    {
        _mapper = mapper;
        _templateRepository = templateRepository;
        _fieldExtractorService = fieldExtractorService;
    }

    public async Task<bool> ChangeTemplateStatusById<T>(int templateId) where T : Models.Template
    {
        var template = await _templateRepository.GetTemplateByIdAsync<T>(templateId);

        template.IsActive = !template.IsActive;

        _templateRepository.UpdateTemplateStatus(template);

        await _templateRepository.SaveChangesAsync();

        return template.IsActive;
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

        await _templateRepository.CreateTemplateAsync(templateModel);
        await _templateRepository.SaveChangesAsync();
    }

    public async Task DeleteTemplateAsync<T>(int templateId) where T : Models.Template
    {
        var template = await _templateRepository.GetTemplateByIdAsync<T>(templateId);

        _templateRepository.DeleteTemplate<T>(template);
    }

    public async Task<IReadOnlyList<TemplateFieldInfoDto>> ExctractFieldsFromTemplateAsync<T>(int templateId) where T : Models.Template
    {
        var template = await _templateRepository.GetTemplateByIdAsync<T>(templateId);
        var fieldsDto = await _fieldExtractorService.ExtractFieldsAsync(template.Path);

        return fieldsDto;
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

        _templateRepository.UpdateTemplate(templateModel);

        await _templateRepository.SaveChangesAsync();
    }

    private T _UpdateTemplate<T>(T template, UpdateTemplateDto templateDto) where T : Models.Template
    {
        template.Title = templateDto.Title;
        template.Path = templateDto.Path;

        return template;
    }
}