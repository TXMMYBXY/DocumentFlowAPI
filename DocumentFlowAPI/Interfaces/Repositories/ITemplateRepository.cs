using DocumentFlowAPI.Base;
using DocumentFlowAPI.Models;

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
    Task<List<T>> GetAllTemplatesAsync<T>() where T : Template;

    /// <summary>
    /// Создает новый шаблон в бд
    /// </summary>
    Task CreateTemplateAsync<T>(T template) where T : Template;

    /// <summary>
    /// Обновляет информацию о шаблоне в таблице
    /// </summary>
    T UpdateTemplate<T>(T template) where T : Template;

    /// <summary>
    /// Обновляет статус шаблона в таблице
    /// </summary>
    T UpdateTemplateStatus<T>(T template) where T : Template;

    /// <summary>
    /// Удаляет шаблон из таблицы
    /// </summary>
    void DeleteTemplate<T>(T template) where T : Template;
}