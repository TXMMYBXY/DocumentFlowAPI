using AutoMapper;
using DocumentFlowAPI.Controllers.User.ViewModels;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.User.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Controllers.User;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserController(IMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    /// <summary>
    /// Получение списка всех пользователей
    /// </summary>
    /// <returns></returns>
    [HttpGet("/get-all")]
    public async Task<ActionResult<List<UserInfoViewModel>>> GetAllUser()
    {
        var listUserDto = await _userService.GetAllUsersAsync();
        var listUserViewModel = _mapper.Map<List<UserInfoViewModel>>(listUserDto);

        return Ok(listUserViewModel);
    }

    //FIXME: После авторизации добавить получение id из клайма
    /// <summary>
    /// Получение информации о пользователе по его Id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("/{userId}/get-user-info")]
    public async Task<ActionResult<UserInfoViewModel>> GetUserByIdAsync([FromRoute] int userId)
    {
        var userDto = await _userService.GetUserByIdAsync(userId);
        var userViewModel = _mapper.Map<UserInfoViewModel>(userDto);

        return Ok(userViewModel);
    }

    /// <summary>
    /// Создание нового пользователя
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost("/add-user")]
    public async Task<ActionResult<NewUserViewModel>> CreateNewUser([FromBody] NewUserViewModel user)
    {
        var userDto = _mapper.Map<NewUserDto>(user);

        await _userService.CreateNewUserAsync(userDto);

        return Ok();
    }

    /// <summary>
    /// Обновление информации о пользователе
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="targetUser"></param>
    /// <returns></returns>
    [HttpPatch("/update-user-info")]
    public async Task<ActionResult> UpdateUserAsync([FromQuery] int userId, [FromBody] UpdateUserInfoViewModel targetUser)
    {
        var userDto = _mapper.Map<UpdateUserInfoDto>(targetUser);
        await _userService.UpdateUserInfoAsync(userId, userDto);

        return Ok();
    }

    /// <summary>
    /// Удаление пользователя
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("/delete-user")]
    public async Task<ActionResult> DeleteUserAsync([FromBody] int userId)
    {
        await _userService.DeleteUserAsync(userId);

        return Ok();
    }
}