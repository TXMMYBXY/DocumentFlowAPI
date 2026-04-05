using System.Text;
using System.Text.Json;
using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.AI;
using DocumentFlowAPI.Services.General;
using DocumentFlowAPI.Services.Template.Dto;
using DocumentFlowAPI.Services.User;
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
    private readonly IFileStorageService _fileStorageService;

    public TemplateService(
        IMapper mapper,
        ITemplateRepository templateRepository,
        IFieldExtractorService fieldExtractorService,
        IContractAiService contractAiService,
        IFileStorageService fileStorageService)
    {
        _mapper = mapper;
        _templateRepository = templateRepository;
        _fieldExtractorService = fieldExtractorService;
        _contractAiService = contractAiService;
        _fileStorageService = fileStorageService;
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
        GeneralService.NullCheck(templateDto, "File is not exists");

        if (templateDto.FileLength == 0)
        {
            throw new ArgumentException("File is empty");
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{templateDto.FileName}";
        var projectFolder = $"{typeof(T)}_{_ClearName(UserIdentity.User.Department)}";

        var filePath = await _fileStorageService.SaveFileAsync(
            templateDto.FileStream,
            uniqueFileName,
            projectFolder);
        
        T templateModel = new T
        {
            Title = templateDto.Title,
            Path = filePath,
            CreatedBy = templateDto.CreatedBy,
            CreatedAt = templateDto.CreatedAt,
            IsActive = templateDto.IsActive
        };

        await _templateRepository.CreateTemplateAsync(templateModel);
        await _templateRepository.SaveChangesAsync();
    }

    public async Task DeleteTemplateAsync<T>(int templateId) where T : Models.Template
    {
        await _templateRepository.DeleteTemplateAsync<T>(templateId);
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

    private static T _UpdateTemplate<T>(T template, UpdateTemplateDto templateDto) where T : Models.Template
    {
        template.Title = templateDto.Title;
        template.Path = templateDto.Path;

        return template;
    }

    public async Task DeleteManyTemplatesAsync<T>(List<int> templateIds) where T : Models.Template
    {
        await _templateRepository.DeleteManyTemplatesAsync<T>(templateIds);
        await _templateRepository.SaveChangesAsync();
    }


    public async Task<DownloadTemplateDto> DownloadTemplateAsync<T>(int templateId) where T : Models.Template, new()
    {
        var template = await _templateRepository.GetTemplateByIdAsync<T>(templateId);

        GeneralService.NullCheck(template, "Document not found");

        return new DownloadTemplateDto
        {
            FilePath = template.Path,
            FileName = template.Title
        };
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

    private string _ClearName(string input)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            input = input.Replace(c, '_');

        return input.Replace(" ", "_");
    }
}