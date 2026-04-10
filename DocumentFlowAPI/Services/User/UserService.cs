using System.Diagnostics;
using System.Text.Json;
using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories.Users;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.General;
using DocumentFlowAPI.Services.User.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace DocumentFlowAPI.Services.User;

public class UserService : IUserService
{
    private const string UsersVersionKey = "users_version";
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IDistributedCache _cache;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        IJwtService jwtService,
        IDistributedCache cache,
        ILogger<UserService> logger
        )
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _jwtService = jwtService;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> ChangeUserStatusByIdAsync(int userId)
    {
        _logger.LogInformation("Changing user status for user with id {UserId}", userId);

        var isActive = await _userRepository.UpdateUserStatusAsync(userId);

        await _userRepository.SaveChangesAsync();

        _logger.LogInformation("User status changed successfully for user with id {UserId}. New status: {IsActive}",
            userId, isActive);

        await _InvalidateUsersCacheAsync();

        return isActive;
    }

    /// <summary>
    /// Создает нового пользователя
    /// </summary>
    /// <param name="newUserDto"></param>
    public async Task CreateNewUserAsync(CreateUserDto newUserDto)
    {
        _logger.LogInformation("Creating new user with email {Email}", newUserDto.Email);

        var userModel = _mapper.Map<Models.User>(newUserDto);
        var userExists = await _userRepository.IsUserAlreadyExists(newUserDto.Email);

        GeneralService.Checker.UniversalCheckException(new GeneralService.CheckerParam<bool>(new ArgumentException("Login already in use"),
            x => x[0], userExists));

        userModel.PasswordHash = new PasswordHasher<Models.User>().HashPassword(userModel, newUserDto.PasswordHash);

        await _userRepository.AddAsync(userModel);
        await _userRepository.SaveChangesAsync();

        var userId = await _userRepository.GetUserByLoginAsync(newUserDto.Email);

        await _jwtService.GenerateRefreshTokenAsync(userId.Id);

        await _InvalidateUsersCacheAsync();

        _logger.LogInformation("User created successfully with email {Email}", newUserDto.Email);
    }

    public async Task DeleteManyUserAsync(List<int> userIds)
    {
        _logger.LogInformation("Deleting multiple users with ids {@UserIds}", userIds);

        await _userRepository.DeleteManyAsync(userIds);
        await _userRepository.SaveChangesAsync();

        await _InvalidateUsersCacheAsync();

        _logger.LogInformation("Users deleted successfully with ids {@UserIds}", userIds);
    }

    /// <summary>
    /// Меняет статус пользователя на заблокированного
    /// </summary>
    public async Task DeleteUserAsync(int userId)
    {
        _logger.LogInformation("Deleting user with id {UserId}", userId);

        await _userRepository.DeleteAsync(userId);
        await _userRepository.SaveChangesAsync();

        await _InvalidateUsersCacheAsync();

        _logger.LogInformation("User deleted successfully with id {UserId}", userId);
    }

    /// <summary>
    /// Возврат всех пользователей
    /// </summary>
    public async Task<PagedUserDto> GetAllUsersAsync(UserFilter userFilter)
    {
        var version = await _GetUsersVersionAsync();
        var serializedFilter = JsonSerializer.Serialize(userFilter);
        var cacheKey = $"users_{version}_{serializedFilter}";

        var cached = await _cache.GetStringAsync(cacheKey);

        if (cached != null)
        {
            return JsonSerializer.Deserialize<PagedUserDto>(cached);
        }

        var listUser = await _userRepository.GetAllUsersAsync(userFilter);
        var listUserDto = _mapper.Map<List<GetUserDto>>(listUser);
        var totalCount = await _userRepository.GetTotalCountAsync();

        var pagedUserDto = new PagedUserDto
        {
            Users = listUserDto,
            TotalCount = totalCount,
            PageSize = userFilter.PageSize ?? totalCount,
            CurrentPage = userFilter.PageNumber ?? 1
        };

        var serializedResult = JsonSerializer.Serialize(pagedUserDto);

        await _cache.SetStringAsync(cacheKey, serializedResult, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });

        return pagedUserDto;
    }

    /// <summary>
    /// Возврат пользователя по id
    /// </summary>
    public async Task<GetUserDto> GetUserByIdAsync(int id)
    {
        var userModel = await _userRepository.GetByIdAsync(id);

        return _mapper.Map<GetUserDto>(userModel);
    }

    /// <summary>
    /// Сброс пароля пользователя
    /// </summary>
    public async Task ResetPasswordAsync(int userId, ResetPasswordDto resetPasswordDto)
    {
        _logger.LogInformation("Resetting password for user with id {UserId}", userId);

        var userModel = await _userRepository.GetByIdAsync(userId);

        userModel.PasswordHash = new PasswordHasher<Models.User>().HashPassword(userModel, resetPasswordDto.PasswordHash);

        _userRepository.UpdateFields(userModel, u => u.PasswordHash);

        await _userRepository.SaveChangesAsync();

        _logger.LogInformation("Password reset successfully for user with id {UserId}", userId);
    }

    /// <summary>
    /// Обновления информации о пользователе
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="userDto"></param>
    /// <returns></returns>
    public async Task UpdateUserPartialAsync(int userId, UpdateUserDto userDto)
    {
        _logger.LogInformation("Updating user with id {UserId}", userId);

        var userModel = await _userRepository.GetByIdAsync(userId);

        GeneralService.NullCheck(userModel, "User does not exist");

        _mapper.Map(userDto, userModel);

        await _userRepository.SaveChangesAsync();

        await _InvalidateUsersCacheAsync();

        _logger.LogInformation("User updated successfully with id {UserId}", userId);
    }

    private async Task<string> _GetUsersVersionAsync()
    {
        var version = await _cache.GetStringAsync(UsersVersionKey);

        if (version == null)
        {
            version = Guid.NewGuid().ToString();

            await _cache.SetStringAsync(UsersVersionKey, version);
        }

        return version;
    }

    private async Task _InvalidateUsersCacheAsync()
    {
        await _cache.SetStringAsync(UsersVersionKey, Guid.NewGuid().ToString());
    }
}