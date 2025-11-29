using DocumentFlowAPI.Services.Template.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface ITemplateService
{
    Task<GetTemplateDto> GetTemplateByIdAsync<T>(int templateId) where T : Models.Template;
    Task<List<GetTemplateDto>> GetAllTemplatesAsync<T>() where T : Models.Template;
    Task CreateTemplateAsync<T>(CreateTemplateDto templateDto) where T : Models.Template, new();
    Task UpdateTemplateAsync<T>(int templateId, UpdateTemplateDto templateDto) where T : Models.Template, new();
    Task DeleteTemplateAsync<T>(int templateId) where T : Models.Template;
}