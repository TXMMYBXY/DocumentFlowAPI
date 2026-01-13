using DocumentFlowAPI.Services.User.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IUserService
{
    /// <summary>
    /// Метод для получения списка всех пользователей в таблице
    /// </summary>
    Task<List<GetUserDto>> GetAllUsersAsync();

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
    Task UpdateUserAsync(int userId, UpdateUserDto userDto);

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

    //TODO: После добавления JobQuartz, добавить метод для очистки таблицы от заблокированных пользователей
}