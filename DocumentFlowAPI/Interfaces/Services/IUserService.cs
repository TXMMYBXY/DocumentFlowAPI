using DocumentFlowAPI.Services.User.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IUserService
{
    Task<List<GetUserDto>> GetAllUsersAsync();
    Task<GetUserDto> GetUserByIdAsync(int id);
    Task CreateNewUserAsync(CreateUserDto newUserDto);
    Task UpdateUserAsync(int userId, UpdateUserDto userDto);
    Task DeleteUserAsync(int userId);
    Task ResetPasswordAsync(int userId, ResetPasswordDto resetPasswordDto);
}