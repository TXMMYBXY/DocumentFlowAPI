using DocumentFlowAPI.Interfaces.Base;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.Template;
using DocumentFlowAPI.Services.Template.Dto;

namespace DocumentFlowAPI.Interfaces.Repositories;

public interface ITemplateRepository : IBaseRepository<Template>
{
    /// <summary>
    /// Возвращает шаблонг по id
    /// </summary>
    Task<T> GetTemplateByIdAsync<T>(int templateId) where T : Template;

    /// <summary>
    /// Возвращает список шаблонов по id
    /// </summary>
    Task<List<T>> GetAllTemplatesAsync<T>(TemplateFilter filter) where T : Template;

    /// <summary>
    /// Создает новый шаблон в бд
    /// </summary>
    Task CreateTemplateAsync<T>(T template) where T : Template;

    /// <summary>
    /// Обновляет информацию о шаблоне в таблице
    /// </summary>
    Task UpdateTemplatePartialAsync<T>(int templateId, string? title, string? path) where T : Template;

    /// <summary>
    /// Обновляет статус шаблона в таблице
    /// </summary>
    Task<bool> UpdateTemplateStatusAsync<T>(int templateId) where T : Template;

    /// <summary>
    /// Удаляет шаблон из таблицы
    /// </summary>
    Task DeleteTemplateAsync<T>(int templateId) where T : Template;

    /// <summary>
    /// Удалять несколько шаблонов из таблицы
    /// </summary>
    Task DeleteManyTemplatesAsync<T>(List<int> templateIds) where T : Template;

    /// <summary>
    /// Возвращает количество шаблонов в таблице
    /// </summary>
    Task<int> GetTotalCountAsync<T>() where T : Template;

    Task<WorkerTemplateDto> GetWorkerTemplateByIdAsync<T>(int templateId) where T : Template;

    Task<string> GetFilePathAsync<T>(int templateId)where T : Template;
}