using DocumentFlowAPI.Base;

namespace DocumentFlowAPI.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<Models.User>
{
    /// <summary>
    /// Добавляет нового пользователя в таблицу
    /// </summary>
    Task CreateNewUserAsync(Models.User userModel);

    /// <summary>
    /// Возвращает пользователя из таблицы по id
    /// </summary>
    Task<Models.User> GetUserByIdAsync(int userId);

    /// <summary>
    /// Обновляет статус пользователя
    /// </summary>
    Models.User UpdateUserStatus(Models.User userModel);

    /// <summary>
    /// Обновляет информацию о пользователе в таблице
    /// </summary>
    Models.User UpdateUser(Models.User userModel);

    /// <summary>
    /// Возвращает пользователя из таблицы по почте
    /// </summary>
    Task<Models.User> GetUserByLoginAsync(string login);

    /// <summary>
    /// Проверяет наличие пользователя в таблице по почте
    /// </summary>
    Task<bool> IsUserAlreadyExists(string email);
    /// <summary>
    /// Удаляет запись о пользователе
    /// </summary>
    void DeleteUser(Models.User user);
}