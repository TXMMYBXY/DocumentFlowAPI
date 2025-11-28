using AutoMapper;
using DocumentFlowAPI.Controllers.Auth.ViewModels;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.Auth.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Controllers.Auth;

[ApiController]
[Route("api/authorization")]
public class AuthorizationController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IAccountService _accountService;

    public AuthorizationController(IMapper mapper, IAccountService accountService)
    {
        _mapper = mapper;
        _accountService = accountService;
    }

    [HttpPost("/register")]
    public async Task<ActionResult<RegisterResponseViewModel>> Register([FromBody] RegisterUserViewModel registerUserViewModel)
    {
        var registerUserDto = _mapper.Map<RegisterUserDto>(registerUserViewModel);
        var registerDto = await _accountService.RegisterAsync(registerUserDto);
        var registerViewModel = _mapper.Map<RegisterResponseViewModel>(registerDto);

        return Ok(registerViewModel);
    }

    [HttpPost("/login")]
    public async Task<ActionResult<LoginResponseViewModel>> Login([FromBody] LoginUserViewModel loginUserViewModel)
    {
        var loginUserDto = _mapper.Map<LoginUserDto>(loginUserViewModel);
        var loginDto = await _accountService.LoginAsync(loginUserDto);
        var loginViewModel = _mapper.Map<LoginResponseViewModel>(loginDto);

        return Ok(loginViewModel);
    } 
}