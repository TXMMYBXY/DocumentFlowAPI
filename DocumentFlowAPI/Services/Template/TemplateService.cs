using System.Text;
using System.Text.Json;
using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.General;
using DocumentFlowAPI.Services.Template.Dto;
using DocumentFlowAPI.Services.User;
using DocumentFlowAPI.Services.WorkerTask.Dto;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Caching.Distributed;

namespace DocumentFlowAPI.Services.Template;

public class TemplateService : ITemplateService
{
    private static bool _isOldVersion = true;
    private readonly IMapper _mapper;
    private readonly ITemplateRepository _templateRepository;
    private readonly IFieldExtractorService _fieldExtractorService;
    private readonly IContractAiService _contractAiService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IDistributedCache _cache;
    private readonly ILogger<TemplateService> _logger;

    public TemplateService(
        IMapper mapper,
        ITemplateRepository templateRepository,
        IFieldExtractorService fieldExtractorService,
        IContractAiService contractAiService,
        IFileStorageService fileStorageService,
        IDistributedCache cache,
        ILogger<TemplateService> logger
        )
    {
        _mapper = mapper;
        _templateRepository = templateRepository;
        _fieldExtractorService = fieldExtractorService;
        _contractAiService = contractAiService;
        _fileStorageService = fileStorageService;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> ChangeTemplateStatusById<T>(int templateId) where T : Models.Template
    {
        _logger.LogInformation("Changing template status for template with id {TemplateId}", templateId);

        var template = await _templateRepository.GetTemplateByIdAsync<T>(templateId);

        template.IsActive = !template.IsActive;

        _templateRepository.UpdateTemplateStatus(template);

        await _templateRepository.SaveChangesAsync();

        _isOldVersion = true;

        _logger.LogInformation("Template status changed successfully for template with id {TemplateId}. New status: {IsActive}",
            templateId, template.IsActive);

        return template.IsActive;
    }

    public async Task CreateTemplateAsync<T>(CreateTemplateDto templateDto) where T : Models.Template, new()
    {
        _logger.LogInformation("Creating new template with title {Title}", templateDto.Title);

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

        _isOldVersion = true;

        _logger.LogInformation("Template created successfully with title {Title}", templateDto.Title);
    }

    public async Task DeleteTemplateAsync<T>(int templateId) where T : Models.Template
    {
        _logger.LogInformation("Deleting template with id {TemplateId}", templateId);

        await _templateRepository.DeleteTemplateAsync<T>(templateId);
        await _templateRepository.SaveChangesAsync();

        _isOldVersion = true;

        _logger.LogInformation("Template deleted successfully with id {TemplateId}", templateId);
    }

    public async Task<List<TemplateFieldInfoDto>> ExctractFieldsFromTemplateAsync<T>(int templateId) where T : Models.Template
    {
        _logger.LogInformation("Extracting fields from template with id {TemplateId}", templateId);

        var template = await _templateRepository.GetTemplateByIdAsync<T>(templateId);
        if (typeof(T) == typeof(Models.ContractTemplate))
        {
            var contractText = _ReadDocx(template.Path);

            var response = _ConvertResponse<List<TemplateFieldInfoDto>>(contractText);

            return response;
        }

        var fieldsDto = await _fieldExtractorService.ExtractFieldsAsync(template.Path);

        _logger.LogInformation("Fields extracted successfully from template with id {TemplateId}. Extracted fields count: {FieldsCount}",
            templateId, fieldsDto.Count);

        return fieldsDto;
    }

    public async Task<PagedTemplateDto> GetAllTemplatesAsync<T>(TemplateFilter templateFilter) where T : Models.Template
    {
        var serializedFilter = JsonSerializer.Serialize(templateFilter);

        lock (serializedFilter)
        {
            if (_isOldVersion)
            {
                _cache.RemoveAsync(serializedFilter).GetAwaiter().GetResult();

                _isOldVersion = false;
            }
        }

        var cached = await _cache.GetStringAsync(serializedFilter);

        if (cached != null)
        {
            return JsonSerializer.Deserialize<PagedTemplateDto>(cached);
        }

        var templates = await _templateRepository.GetAllTemplatesAsync<T>(templateFilter);
        var listTemplateDto = _mapper.Map<List<GetTemplateDto>>(templates);
        var totalCount = await _templateRepository.GetTotalCountAsync<T>();

        var pagedTemplateDto = new PagedTemplateDto
        {
            Templates = listTemplateDto,
            TotalCount = totalCount,
            PageSize = templateFilter.PageSize ?? totalCount,
            CurrentPage = templateFilter.PageNumber ?? 1
        };

        var serializedResult = JsonSerializer.Serialize(pagedTemplateDto);

        await _cache.SetStringAsync(serializedFilter, serializedResult, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });

        return pagedTemplateDto;
    }

    public async Task<GetTemplateForWorkerDto> GetTemplateForWorkerByIdAsync<T>(int templateId) where T : Models.Template
    {
        var temaplate = await _templateRepository.GetWorkerTemplateByIdAsync<T>(templateId);

        return _mapper.Map<GetTemplateForWorkerDto>(temaplate);
    }

    public async Task UpdateTemplateAsync<T>(int templateId, UpdateTemplateDto templateDto) where T : Models.Template, new()
    {
        _logger.LogInformation("Updating template with id {TemplateId}", templateId);

        var template = await _templateRepository.GetTemplateByIdAsync<T>(templateId);
        var templateModel = _UpdateTemplate(template, templateDto);

        _templateRepository.UpdateTemplate(templateModel);

        await _templateRepository.SaveChangesAsync();

        _isOldVersion = true;

        _logger.LogInformation("Template updated successfully with id {TemplateId}", templateId);
    }

    private static T _UpdateTemplate<T>(T template, UpdateTemplateDto templateDto) where T : Models.Template
    {
        template.Title = templateDto.Title;
        template.Path = templateDto.Path;

        return template;
    }

    public async Task DeleteManyTemplatesAsync<T>(List<int> templateIds) where T : Models.Template
    {
        _logger.LogInformation("Deleting multiple templates with ids {TemplateIds}", string.Join(", ", templateIds));

        await _templateRepository.DeleteManyTemplatesAsync<T>(templateIds);
        await _templateRepository.SaveChangesAsync();

        _isOldVersion = true;

        _logger.LogInformation("Templates deleted successfully with ids {TemplateIds}", string.Join(", ", templateIds));
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