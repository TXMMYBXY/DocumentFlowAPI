using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.User.Dto;

namespace DocumentFlowAPI.Services.User;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task CreateNewUserAsync(NewUserDto newUserDto)
    {
        var userModel = _mapper.Map<Models.User>(newUserDto);

        await _userRepository.CreateNewUserAsync(userModel);
        await _userRepository.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _userRepository.GetEntityById(userId);

        user.IsActive = false;

        _userRepository.UpdateUserStatus(user);

        await _userRepository.SaveChangesAsync();
    }

    public async Task<List<UserInfoDto>> GetAllUsersAsync()
    {
        var userModelList = await _userRepository.GetAllAsync();

        return _mapper.Map<List<UserInfoDto>>(userModelList);
    }

    public async Task<UserInfoDto> GetUserByIdAsync(int id)
    {
        var userModel = await _userRepository.GetUserByIdAsync(id);

        return _mapper.Map<UserInfoDto>(userModel);
    }

    public async Task UpdateUserInfoAsync(int userId, UpdateUserInfoDto userDto)
    {
        var userModel = await _userRepository.GetUserByIdAsync(userId);
        var updateUser = _UpdateUser(userModel, userDto);

        _userRepository.Update(updateUser);

        await _userRepository.SaveChangesAsync();
    }
    
    private Models.User _UpdateUser(Models.User user, UpdateUserInfoDto userDto)
    {
        user.FullName = userDto.FullName;
        user.Email = userDto.Email;
        user.DepartmentId = userDto.DepartmentId;
        user.RoleId = userDto.RoleId;

        return user;
    }
}