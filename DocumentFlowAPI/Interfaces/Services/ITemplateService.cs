using DocumentFlowAPI.Services.Template.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface ITemplateService
{
    Task<TemplateDto> GetTemplateByIdAsync<T>(int templateId) where T : Models.Template;
    Task<List<TemplateDto>> GetAllTemplatesAsync<T>() where T : Models.Template;
    Task CreateTemplateAsync<T>(NewTemplateDto templateDto) where T : Models.Template, new();
    Task UpdateTemplateAsync<T>(int templateId, UpdateTemplateDto templateDto) where T : Models.Template, new();
    Task DeleteTemplateAsync<T>(int templateId) where T : Models.Template;
}