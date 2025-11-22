using DocumentFlowAPI.Services.User.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IUserService
{
    Task CreateNewUserAsync(NewUserDto newUserDto);
    Task<UserInfoDto> GetUserByIdAsync(int id);
    Task UpdateUserInfoAsync(int userId, UpdateUserInfoDto userDto);
    Task DeleteUserAsync(int userId);
    Task<List<UserInfoDto>> GetAllUsersAsync();
}