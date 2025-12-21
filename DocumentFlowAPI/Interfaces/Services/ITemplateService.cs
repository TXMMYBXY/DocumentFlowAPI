using DocumentFlowAPI.Services.Template.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface ITemplateService
{
    /// <summary>
    /// Получения шаблона по id
    /// </summary>
    Task<GetTemplateDto> GetTemplateByIdAsync<T>(int templateId) where T : Models.Template;

    /// <summary>
    /// Получение списка всех шаблонов
    /// </summary>
    Task<List<GetTemplateDto>> GetAllTemplatesAsync<T>() where T : Models.Template;

    /// <summary>
    /// Создание нового шаблона
    /// </summary>
    Task CreateTemplateAsync<T>(CreateTemplateDto templateDto) where T : Models.Template, new();

    /// <summary>
    /// Изменения данных о шаблоне
    /// </summary>
    Task UpdateTemplateAsync<T>(int templateId, UpdateTemplateDto templateDto) where T : Models.Template, new();

    /// <summary>
    /// Метод для смены статуса шаблона
    /// </summary>
    Task DeleteTemplateAsync<T>(int templateId) where T : Models.Template;

    //TODO: После добавления JobQuartz, добавить метод для очистки таблицы от заблокированных шаблонов

}