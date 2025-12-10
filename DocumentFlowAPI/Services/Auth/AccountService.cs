using AutoMapper;
using DocumentFlowAPI.Configuration;
using DocumentFlowAPI.Controllers.Auth.ViewModels;
using DocumentFlowAPI.Controllers.User.ViewModels;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.Auth.Dto;
using DocumentFlowAPI.Services.General;
using DocumentFlowAPI.Services.User.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;

namespace DocumentFlowAPI.Services.Auth;

public class AccountService : GeneralService, IAccountService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public AccountService(
        IMapper mapper,
        IUserRepository userRepository,
        IJwtService jwtService,
        IOptions<JwtSettings> jwtSettings)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginUserDto loginUserDto)
    {
        var user = await _userRepository.GetUserByLoginAsync(loginUserDto.Email);
        var result = new PasswordHasher<Models.User>().VerifyHashedPassword(user, user.PasswordHash, loginUserDto.PasswordHash);
        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresDays);
        var checker = new Checker();

        Checker.UniversalCheck(new CheckerParam<Models.User>(new ArgumentException("Incorrect login"),
            x => x[0] == null, user));

        Checker.UniversalCheck(new CheckerParam<PasswordVerificationResult>(new ArgumentException("Incorrect password"),
            x => x[0] != PasswordVerificationResult.Success, result));

        Checker.UniversalCheck(new CheckerParam<Models.User>(new ArgumentException("User was deleted"),
            x => !x[0].IsActive, user));

        return new LoginResponseDto
        {
            UserInfo = _mapper.Map<UserInfoForLoginDto>(user),
            Token = _jwtService.GenerateToken(user),
            ExpiresAt = _jwtSettings.ExpiresDays.ToString()
        };
    }
}
