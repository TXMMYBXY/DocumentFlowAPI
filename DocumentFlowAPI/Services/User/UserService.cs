using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.User.Dto;

namespace DocumentFlowAPI.Services.User;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CreateNewUserAsync(NewUserDto newUserDto)
    {
        var userModel = new Models.User
        {
            Login = newUserDto.Login,
            PasswordHash = newUserDto.PasswordHash,
            FullName = "",
            Email = "",
            DepartmentId = newUserDto.DepartmentId,
            RoleId = newUserDto.DepartmentId
        };

        await _userRepository.CreateNewUserAsync(userModel);
        await _userRepository.SaveChangesAsync();
    }
}