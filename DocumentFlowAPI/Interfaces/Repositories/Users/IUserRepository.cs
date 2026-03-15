using DocumentFlowAPI.Interfaces.Base;
using DocumentFlowAPI.Interfaces.Repositories.Users.Dtos;
using DocumentFlowAPI.Services.User;

namespace DocumentFlowAPI.Interfaces.Repositories.Users;

public interface IUserRepository : IBaseRepository<Models.User>
{
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

    Task<List<UserDto>> GetAllUsersAsync(UserFilter filter);
}