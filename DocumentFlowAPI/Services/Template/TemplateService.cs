using System.Text;
using System.Text.Json;
using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.AI;
using DocumentFlowAPI.Services.Template.Dto;
using DocumentFlowAPI.Services.WorkerTask.Dto;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocumentFlowAPI.Services.Template;

public class TemplateService : ITemplateService
{
    private readonly IMapper _mapper;
    private readonly ITemplateRepository _templateRepository;
    private readonly IFieldExtractorService _fieldExtractorService;
    private readonly IContractAiService _contractAiService;

    public TemplateService(
        IMapper mapper,
        ITemplateRepository templateRepository,
        IFieldExtractorService fieldExtractorService,
        IContractAiService contractAiService)
    {
        _mapper = mapper;
        _templateRepository = templateRepository;
        _fieldExtractorService = fieldExtractorService;
        _contractAiService = contractAiService;
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
        
        await _templateRepository.SaveChangesAsync();
    }

    public async Task<List<TemplateFieldInfoDto>> ExctractFieldsFromTemplateAsync<T>(int templateId) where T : Models.Template
    {
        var template = await _templateRepository.GetTemplateByIdAsync<T>(templateId);
        if (typeof(T) == typeof(Models.ContractTemplate))
        {
            var contractText = _ReadDocx(template.Path);

            var response = _ConvertResponse<List<TemplateFieldInfoDto>>(contractText);

            return response;
        }
        
        var fieldsDto = await _fieldExtractorService.ExtractFieldsAsync(template.Path);

        return fieldsDto;
    }

    private async Task<List<TemplateFieldInfoDto>> _ExctractFieldsByAiAsync<T>(int templateId) where T : Models.ContractTemplate
    {
        var template = await _templateRepository.GetTemplateByIdAsync<T>(templateId);
        var contractText = _ReadDocx(template.Path);
        var jsonResponse = await _contractAiService.ExtractFieldsJsonAsync(contractText);
        var response = _ConvertResponse<List<TemplateFieldInfoDto>>(jsonResponse);

        return response;
    }

    private static string _ReadDocx(string filePath)
    {
        StringBuilder sb = new StringBuilder();

        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
        {
            Body body = wordDoc.MainDocumentPart.Document.Body;
            foreach (var para in body.Elements<Paragraph>())
            {
                sb.AppendLine(para.InnerText);
            }
        }

        return sb.ToString();
    }

    private static T? _ConvertResponse<T>(string response)
    {
        if (response.Equals(""))
        {
            return default;
        }
        
        return JsonSerializer.Deserialize<T>(response);
    }

    public async Task<PagedTemplateDto> GetAllTemplatesAsync<T>(TemplateFilter templateFilter) where T : Models.Template
    {
        var templates = await _templateRepository.GetAllTemplatesAsync<T>(templateFilter);
        var listTemplateDto = _mapper.Map<List<GetTemplateDto>>(templates);
        var totalCount = await _templateRepository.GetTotalCountAsync<T>();

        return new PagedTemplateDto
        {
            Templates = listTemplateDto,
            TotalCount = totalCount,
            PageSize = templateFilter.PageSize ?? totalCount,
            CurrentPage = templateFilter.PageNumber ?? 1
        };
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