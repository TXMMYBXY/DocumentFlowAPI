using System.Linq.Expressions;

namespace DocumentFlowAPI.Interfaces.Base;

public interface IBaseRepository<T> where T : class
{
    /// <summary>
    /// Возврат всех записей
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Возврат записи по id
    /// </summary>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Добавление записи
    /// </summary>
    Task AddAsync(T entity);

    /// <summary>
    /// Обновление записи
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// Удаление записи
    /// </summary>
    Task DeleteAsync(int id);

    /// <summary>
    /// Множественное удаление записей
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task DeleteManyAsync(List<int> ids);

    /// <summary>
    /// Сохранение изменений в таблицах
    /// </summary>
    Task SaveChangesAsync();

    /// <summary>
    /// Обновление записей в таблице
    /// </summary>
    /// <param name="entity">Сущность, у которой надо изменить свойства</param>
    /// <param name="fields">Свойства, которые меняются(в виде функции)</param>
    void UpdateFields(T entity, params Expression<Func<T, object>>[] fields);
}