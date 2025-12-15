using AutoMapper;
using DocumentFlowAPI.Controllers.Auth.ViewModels;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.Auth.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    /// <summary>
    /// че бубнить...
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseViewModel>> Login([FromBody] LoginRequestViewModel loginUserViewModel)
    {
        var loginUserDto = _mapper.Map<LoginUserDto>(loginUserViewModel);
        var loginDto = await _accountService.LoginAsync(loginUserDto);
        var loginViewModel = _mapper.Map<LoginResponseViewModel>(loginDto);

        return Ok(loginViewModel);
    }
    /// <summary>
    /// Метод для обновления рефреш токена(сам генерирую)
    /// </summary>
    /// <param name="tokenViewModel">Старый рефреш токен</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("refresh")]
    public async Task<ActionResult<RefreshTokenResponseViewModel>> RefreshToken([FromBody] RefreshTokenRequestViewModel tokenViewModel)
    {
        tokenViewModel.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var tokenDto = _mapper.Map<RefreshTokenDto>(tokenViewModel);
        var tokenResponseDto = await _accountService.RefreshAsync(tokenDto);
        var tokenResponseViewModel = _mapper.Map<RefreshTokenResponseViewModel>(tokenResponseDto);

        return Ok(tokenResponseViewModel);
    }
    /// <summary>
    /// Метод для обновления токена доступа(у меня JWT)
    /// </summary>
    /// <param name="tokenViewModel"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("access")]
    public async Task<ActionResult<AccessTokenResponseViewModel>> GetAccessTokenAsync([FromBody] AccessTokenViewModel tokenViewModel)
    {
        tokenViewModel.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var tokenDto = _mapper.Map<AccessTokenDto>(tokenViewModel);
        var tokenResponseDto = await _accountService.CreateAccessTokenAsync(tokenDto);
        var tokenResponse = _mapper.Map<AccessTokenResponseViewModel>(tokenResponseDto);

        return Ok(tokenResponse);
    }
}