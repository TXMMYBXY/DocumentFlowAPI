using System.ComponentModel;
using System.Security.Claims;
using AutoMapper;
using DocumentFlowAPI.Controllers.Auth;
using DocumentFlowAPI.Controllers.User.ViewModels;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.User.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Controllers.User;

[ApiController]
[Route("api/users")]
[AuthorizeByRoleId((int)Permissions.Admin)]
///Этим контроллером будет пользоваться администратор, поэтому информация которую он получает - полная
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
    [HttpGet("get-all")]
    public async Task<ActionResult<List<GetUserViewModel>>> GetAllUser()
    {
        var listUserDto = await _userService.GetAllUsersAsync();
        var listUserViewModel = _mapper.Map<List<GetUserViewModel>>(listUserDto);

        return Ok(listUserViewModel);
    }

    /// <summary>
    /// Получение информации о пользователе по его Id
    /// </summary>
    /// <param name="targetUserId"></param>
    /// <returns></returns>
    [HttpGet("{userId}/get-user-info")]
    public async Task<ActionResult<GetUserViewModel>> GetUserByIdAsync([FromRoute] int targetUserId)
    {
        var userDto = await _userService.GetUserByIdAsync(targetUserId);
        var userViewModel = _mapper.Map<GetUserViewModel>(userDto);

        return Ok(userViewModel);
    }

    /// <summary>
    /// Создание нового пользователя
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost("add-user")]
    public async Task<ActionResult<CreateUserViewModel>> CreateNewUser([FromBody] CreateUserViewModel user)
    {
        var userDto = _mapper.Map<CreateUserDto>(user);

        await _userService.CreateNewUserAsync(userDto);

        return Created();
    }

    /// <summary>
    /// Обновление информации о пользователе
    /// </summary>
    /// <param name="targetUserId"></param>
    /// <param name="userViewModel"></param>
    /// <returns></returns>
    [HttpPatch("{userId}/update-user-info")]
    public async Task<ActionResult> UpdateUserAsync([FromRoute] int targetUserId, [FromBody] UpdateUserViewModel userViewModel)
    {
        var userDto = _mapper.Map<UpdateUserDto>(userViewModel);

        await _userService.UpdateUserAsync(targetUserId, userDto);

        return Ok();
    }

    [HttpPatch("{userId}/reset-password")]
    public async Task<ActionResult> ResetPasswordAsync([FromRoute] int userId, ResetPasswordViewModel resetPasswordViewModel)
    {
        var resetPasswordDto = _mapper.Map<ResetPasswordDto>(resetPasswordViewModel);
        await _userService.ResetPasswordAsync(userId, resetPasswordDto);

        return Ok();
    }

    /// <summary>
    /// Удаление пользователя
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("delete-user")]
    public async Task<ActionResult> DeleteUserAsync([FromBody] DeleteUserViewModel userViewModel)
    {
        await _userService.DeleteUserAsync(userViewModel.UserId);

        return Ok();
    }
}