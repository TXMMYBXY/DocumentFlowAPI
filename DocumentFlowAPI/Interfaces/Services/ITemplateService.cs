using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.Template.Dto;
using DocumentFlowAPI.Services.WorkerTask.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface ITemplateService
{
    /// <summary>
    /// Получения шаблона по id
    /// </summary>
    Task<GetTemplateDto> GetTemplateByIdAsync<T>(int templateId) where T : Template;

    /// <summary>
    /// Получение списка всех шаблонов
    /// </summary>
    Task<List<GetTemplateDto>> GetAllTemplatesAsync<T>() where T : Template;

    /// <summary>
    /// Создание нового шаблона
    /// </summary>
    Task CreateTemplateAsync<T>(CreateTemplateDto templateDto) where T : Template, new();

    /// <summary>
    /// Изменения данных о шаблоне
    /// </summary>
    Task UpdateTemplateAsync<T>(int templateId, UpdateTemplateDto templateDto) where T : Template, new();

    /// <summary>
    /// Метод для удаления шаблона
    /// </summary>
    Task DeleteTemplateAsync<T>(int templateId) where T : Template;

    /// <summary>
    /// Метод для смены статуса шаблона
    /// </summary>
    Task<bool> ChangeTemplateStatusById<T>(int templateId) where T : Template;

    /// <summary>
    /// Метод для извелчения полей из шаблона
    /// </summary>
    Task<IReadOnlyList<TemplateFieldInfoDto>> ExctractFieldsFromTemplateAsync<T>(int templateId) where T : Template;

    //TODO: После добавления JobQuartz, добавить метод для очистки таблицы от заблокированных шаблонов

}