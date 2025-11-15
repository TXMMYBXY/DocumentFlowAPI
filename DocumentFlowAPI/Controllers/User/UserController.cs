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

    [HttpGet("/all")]
    public async Task<ActionResult<List<UserInfoViewModel>>> GetAllUser()
    {
        return null;
    }

    [HttpPost("/add-user")]
    public async Task<ActionResult<NewUserViewModel>> CreateNewUser([FromBody] NewUserViewModel user)
    {
        var userDto = _mapper.Map<NewUserDto>(user);

        await _userService.CreateNewUserAsync(userDto);

        return Ok();
    }
}