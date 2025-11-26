using DocumentFlowAPI.Base;

namespace DocumentFlowAPI.Interfaces.Repositories;

public interface ITemplateRepository : IBaseRepository<Models.Template>
{
    Task<T> GetTemplateByIdAsync<T>(int templateId) where T : Models.Template;
    Task<List<T>> GetAllTemplatesAsync<T>() where T : Models.Template;
    Task CreateTemplateAsync<T>(T template) where T : Models.Template;
    T UpdateTemplate<T>(T template) where T : Models.Template;
    T UpdateTemplateStatus<T>(T template) where T : Models.Template;
}