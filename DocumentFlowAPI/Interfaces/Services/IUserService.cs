using DocumentFlowAPI.Services.User;
using DocumentFlowAPI.Services.User.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IUserService
{
    /// <summary>
    /// Метод для получения списка всех пользователей в таблице
    /// </summary>
    Task<PagedUserDto> GetAllUsersAsync(UserFilter userFilter);

    /// <summary>
    /// Метод для получения пользователя по id
    /// </summary>
    Task<GetUserDto> GetUserByIdAsync(int id);

    /// <summary>
    /// Метод для создания нового пользователя
    /// </summary>
    Task CreateNewUserAsync(CreateUserDto newUserDto);

    /// <summary>
    /// Мето для обновления информации о пользователе
    /// </summary>
    Task UpdateUserPartialAsync(int userId, UpdateUserDto userDto);

    /// <summary>
    /// Метод для блокировки пользователя
    /// </summary>
    Task DeleteUserAsync(int userId);

    /// <summary>
    /// Метод для смены пароля пользователя
    /// </summary>
    Task ResetPasswordAsync(int userId, ResetPasswordDto resetPasswordDto);

    /// <summary>
    /// Метод для смены статуса пользователя на противоположный
    /// </summary>
    Task<bool> ChangeUserStatusByIdAsync(int userId);

    /// <summary>
    /// Метод для удаления нескольких пользователей по их id
    /// </summary>
    Task DeleteManyUserAsync(List<int> userIds);
}