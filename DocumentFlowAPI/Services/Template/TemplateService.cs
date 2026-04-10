using System.Text;
using System.Text.Json;
using AutoMapper;
using DocumentFlowAPI.Enums;
using DocumentFlowAPI.Hubs;
using DocumentFlowAPI.Hubs.Notifiaction;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.General;
using DocumentFlowAPI.Services.Template.Dto;
using DocumentFlowAPI.Services.User;
using DocumentFlowAPI.Services.WorkerTask.Dto;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

namespace DocumentFlowAPI.Services.Template;

public class TemplateService : ITemplateService
{
    private const string TemplatesVersionKey = "templates_version";
    private const string FieldsVersionKey = "fields_version";
    private readonly IMapper _mapper;
    private readonly ITemplateRepository _templateRepository;
    private readonly IFieldExtractorService _fieldExtractorService;
    private readonly IContractAiService _contractAiService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IDistributedCache _cache;
    private readonly ILogger<TemplateService> _logger;
    private readonly INotificationService _notificationService;

    public TemplateService(
        IMapper mapper,
        ITemplateRepository templateRepository,
        IFieldExtractorService fieldExtractorService,
        IContractAiService contractAiService,
        IFileStorageService fileStorageService,
        IDistributedCache cache,
        ILogger<TemplateService> logger,
        INotificationService notificationService)
    {
        _mapper = mapper;
        _templateRepository = templateRepository;
        _fieldExtractorService = fieldExtractorService;
        _contractAiService = contractAiService;
        _fileStorageService = fileStorageService;
        _cache = cache;
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task<bool> ChangeTemplateStatusById<T>(int templateId) where T : Models.Template
    {
        _logger.LogInformation("Changing template status for template with id {TemplateId}", templateId);

        var isActive = await _templateRepository.UpdateTemplateStatusAsync<T>(templateId);

        await _templateRepository.SaveChangesAsync();

        _logger.LogInformation("Template status changed successfully for template with id {TemplateId}. New status: {IsActive}",
            templateId, isActive);

        await _InvalidateTemplatesCacheAsync();
        
        _logger.LogDebug("Sending notification");

        return isActive;
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

        await _InvalidateTemplatesCacheAsync();
        
        _logger.LogDebug("Sending notification");
        
        await _notificationService.SendNotificationToAllAsync(new NotificationDto(
            NotificationKind.TemplateAdded,
            NotificationSeverity.Info,
            "Новый шаблон добавлен",
            $"Добавлен шаблон {templateDto.Title}"
            ));

        _logger.LogInformation("Template created successfully with title {Title}", templateDto.Title);
    }

    public async Task DeleteTemplateAsync<T>(int templateId) where T : Models.Template
    {
        _logger.LogInformation("Deleting template with id {TemplateId}", templateId);

        await _templateRepository.DeleteTemplateAsync<T>(templateId);
        await _templateRepository.SaveChangesAsync();

        await _InvalidateTemplatesCacheAsync();

        _logger.LogInformation("Template deleted successfully with id {TemplateId}", templateId);
    }

    public async Task<List<TemplateFieldInfoDto>> ExtractFieldsFromTemplateAsync<T>(int templateId) where T : Models.Template
    {
        _logger.LogInformation("Extracting fields from template with id {TemplateId}", templateId);

        var version = await _GetFieldsVersionAsync();
        var targetTemplate = JsonSerializer.Serialize(templateId);
        var cacheKey = $"users_{version}_{targetTemplate}";

        var cached = await _cache.GetStringAsync(cacheKey);

        if (cached != null)
        {
            return JsonSerializer.Deserialize<List<TemplateFieldInfoDto>>(cached);
        }

        var template = await _templateRepository.GetTemplateByIdAsync<T>(templateId);

        if (typeof(T) == typeof(Models.ContractTemplate))
        {
            var contractText = _ReadDocx(template.Path);

            var response = _ConvertResponse<List<TemplateFieldInfoDto>>(contractText);

            return response;
        }

        var fieldsDto = await _fieldExtractorService.ExtractFieldsAsync(template.Path);

        _logger.LogInformation("Fields extracted successfully from template with id {TemplateId}. Extracted fields count: {FieldsCount}",
            targetTemplate, (object)fieldsDto.Count);

        var serializedResult = JsonSerializer.Serialize(fieldsDto);

        await _cache.SetStringAsync(cacheKey, serializedResult, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });

        return fieldsDto;
    }

    public async Task<PagedTemplateDto> GetAllTemplatesAsync<T>(TemplateFilter templateFilter) where T : Models.Template
    {
        var version = await _GetTemplatesVersionAsync();
        var serializedFilter = JsonSerializer.Serialize(templateFilter);
        var cacheKey = $"users_{version}_{serializedFilter}";

        var cached = await _cache.GetStringAsync(cacheKey);

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

        await _cache.SetStringAsync(cacheKey, serializedResult, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });

        return pagedTemplateDto;
    }

    public async Task<GetTemplateForWorkerDto> GetTemplateForWorkerByIdAsync<T>(int templateId) where T : Models.Template
    {
        var template = await _templateRepository.GetWorkerTemplateByIdAsync<T>(templateId);

        return _mapper.Map<GetTemplateForWorkerDto>(template);
    }

    public async Task UpdateTemplatePartialAsync<T>(int templateId, UpdateTemplateDto templateDto) where T : Models.Template, new()
    {
        _logger.LogInformation("Updating template with id {TemplateId}", templateId);
        
        string filePath = null;
        
        if (templateDto.FileStream != null && templateDto.FileLength != 0)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{templateDto.FileName}";
            var projectFolder = $"{typeof(T)}_{_ClearName(UserIdentity.User.Department)}";
        
            var oldFilePath = await _templateRepository.GetFilePathAsync<T>(templateId);
        
            await _fileStorageService.DeleteFileAsync(oldFilePath);
        
            filePath = await _fileStorageService.SaveFileAsync(
                templateDto.FileStream,
                uniqueFileName,
                projectFolder);
        }
        
        await _templateRepository.UpdateTemplatePartialAsync<T>(templateId, templateDto.Title, filePath);
        await _templateRepository.SaveChangesAsync();

        await _InvalidateTemplatesCacheAsync();

        await _notificationService.SendNotificationToAllAsync(new NotificationDto(NotificationKind.TemplateUpdated,
            NotificationSeverity.Info,
            "Шаблон изменен", $"Шаблон номер {templateId} изменен"));
        
        _logger.LogInformation("Template updated successfully with id {TemplateId}", templateId);
    }
    
    public async Task DeleteManyTemplatesAsync<T>(List<int> templateIds) where T : Models.Template
    {
        _logger.LogInformation("Deleting multiple templates with ids {TemplateIds}", string.Join(", ", templateIds));

        await _templateRepository.DeleteManyTemplatesAsync<T>(templateIds);
        await _templateRepository.SaveChangesAsync();

        await _InvalidateTemplatesCacheAsync();

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
        var version = await _GetFieldsVersionAsync();
        var targetTemplate = JsonSerializer.Serialize(templateId);
        var cacheKey = $"users_{version}_{targetTemplate}";

        var cached = await _cache.GetStringAsync(cacheKey);

        if (cached != null)
        {
            return JsonSerializer.Deserialize<List<TemplateFieldInfoDto>>(cached);
        }
        
        var template = await _templateRepository.GetTemplateByIdAsync<T>(templateId);
        var contractText = _ReadDocx(template.Path);
        var jsonResponse = await _contractAiService.ExtractFieldsJsonAsync(contractText);

        await _cache.SetStringAsync(cacheKey, jsonResponse, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });

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

    private async Task<string> _GetTemplatesVersionAsync()
    {
        var version = await _cache.GetStringAsync(TemplatesVersionKey);

        if (version == null)
        {
            version = Guid.NewGuid().ToString();

            await _cache.SetStringAsync(TemplatesVersionKey, version);
        }

        return version;
    }

    private async Task<string> _GetFieldsVersionAsync()
    {
        var version = await _cache.GetStringAsync(FieldsVersionKey);

        if (version == null)
        {
            version = Guid.NewGuid().ToString();

            await _cache.SetStringAsync(FieldsVersionKey, version);
        }

        return version;
    }

    private async Task _InvalidateTemplatesCacheAsync()
    {
        await _cache.SetStringAsync(TemplatesVersionKey, Guid.NewGuid().ToString());
        await _cache.SetStringAsync(FieldsVersionKey, Guid.NewGuid().ToString());
    }
}