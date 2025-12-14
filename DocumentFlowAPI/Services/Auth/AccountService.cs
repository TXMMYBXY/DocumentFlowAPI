using AutoMapper;
using DocumentFlowAPI.Configuration;
using DocumentFlowAPI.Controllers.Auth.ViewModels;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Models.AboutUserModels;
using DocumentFlowAPI.Services.Auth.Dto;
using DocumentFlowAPI.Services.General;
using DocumentFlowAPI.Services.User;
using DocumentFlowAPI.Services.User.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DocumentFlowAPI.Services.Auth;

public class AccountService : GeneralService, IAccountService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public AccountService(
        IMapper mapper,
        IUserRepository userRepository,
        ITokenRepository tokenRepository,
        IJwtService jwtService,
        IOptions<JwtSettings> jwtSettings)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
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
            AccessToken = _jwtService.GenerateAccessToken(user),
            ExpiresAt = _jwtSettings.ExpiresDays.ToString(),
            RefreshToken = _jwtService.GenerateRefreshToken(user.Id)
        };
    }

    public async Task<RefreshTokenResponseDto> RefreshAsync(RefreshTokenDto refreshTokenDto)
    {
        var refreshTokenModel = _mapper.Map<RefreshToken>(refreshTokenDto);
        var isValid = _jwtService.ValidateRefreshToken(refreshTokenModel);

        Checker.UniversalCheck(new CheckerParam<RefreshToken>(new NullReferenceException("Incorrect token"),
            x => !isValid.Result == true, refreshTokenModel));
        
        var refreshToken = await _tokenRepository.GetRefreshTokenByUserIdAsync(UserIdentity.User.Id);

        _jwtService.RefreshTokenValue(refreshToken);

        await _tokenRepository.SaveChangesAsync();

        var refreshTokenResponseDto = _mapper.Map<RefreshTokenResponseDto>(refreshToken);

        return refreshTokenResponseDto;
    }
}
