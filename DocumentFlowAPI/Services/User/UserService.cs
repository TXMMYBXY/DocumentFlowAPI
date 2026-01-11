using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.General;
using DocumentFlowAPI.Services.User.Dto;
using Microsoft.AspNetCore.Identity;

namespace DocumentFlowAPI.Services.User;

public class UserService : GeneralService, IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _jwtService = jwtService;
    }
    /// <summary>
    /// Создает нового пользователя
    /// </summary>
    /// <param name="newUserDto"></param>
    public async Task CreateNewUserAsync(CreateUserDto newUserDto)
    {
        var userModel = _mapper.Map<Models.User>(newUserDto);
        var userExists = await _userRepository.IsUserAlreadyExists(newUserDto.Email);

        Checker.UniversalCheck(new CheckerParam<bool>(new ArgumentException("Login already in use"),
            x => x[0], userExists));

        userModel.PasswordHash = new PasswordHasher<Models.User>().HashPassword(userModel, newUserDto.PasswordHash);

        await _userRepository.CreateNewUserAsync(userModel);
        await _userRepository.SaveChangesAsync();

        var userId = await _userRepository.GetUserByLoginAsync(newUserDto.Email);

        await _jwtService.GenerateRefreshTokenAsync(userId.Id);
    }

    /// <summary>
    /// Меняет статус пользователя на заблокированного
    /// </summary>
    public async Task DeleteUserAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        user.IsActive = false;

        _userRepository.UpdateUserStatus(user);

        await _userRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Возврат всех пользователей
    /// </summary>
    public async Task<List<GetUserDto>> GetAllUsersAsync()
    {
        var userModelList = await _userRepository.GetAllAsync();

        return _mapper.Map<List<GetUserDto>>(userModelList);
    }

    /// <summary>
    /// Возврат пользователя по id
    /// </summary>
    public async Task<GetUserDto> GetUserByIdAsync(int id)
    {
        var userModel = await _userRepository.GetUserByIdAsync(id);

        return _mapper.Map<GetUserDto>(userModel);
    }

    /// <summary>
    /// Сброс пароля пользователя
    /// </summary>
    public async Task ResetPasswordAsync(int userId, ResetPasswordDto resetPasswordDto)
    {
        var userModel = await _userRepository.GetUserByIdAsync(userId);

        userModel.PasswordHash = new PasswordHasher<Models.User>().HashPassword(userModel, resetPasswordDto.PasswordHash);

        _userRepository.UpdateFields(userModel, u => u.PasswordHash);
        await _userRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Обновления информации о пользователе
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="userDto"></param>
    /// <returns></returns>
    public async Task UpdateUserAsync(int userId, UpdateUserDto userDto)
    {
        var userModel = await _userRepository.GetUserByIdAsync(userId);
        var updateUser = _UpdateUser(userModel, userDto);

        _userRepository.UpdateUser(updateUser);

        await _userRepository.SaveChangesAsync();
    }

    private Models.User _UpdateUser(Models.User user, UpdateUserDto userDto)
    {
        user.FullName = userDto.FullName;
        user.Email = userDto.Email;
        user.Department = userDto.Department;
        user.RoleId = userDto.RoleId;

        return user;
    }
}