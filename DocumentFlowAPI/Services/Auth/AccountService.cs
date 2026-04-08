using AutoMapper;
using DocumentFlowAPI.Configuration;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Repositories.Users;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Models.AboutUserModels;
using DocumentFlowAPI.Services.Auth.Dto;
using DocumentFlowAPI.Services.General;
using DocumentFlowAPI.Services.Personal.Dto;
using DocumentFlowAPI.Services.User.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DocumentFlowAPI.Services.Auth;

public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;
    private readonly IRefreshTokenHasher _refreshTokenHasher;
    private readonly IPersonalAccountService _personalAccountService;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        IMapper mapper,
        IUserRepository userRepository,
        ITokenRepository tokenRepository,
        IJwtService jwtService,
        IOptions<JwtSettings> jwtSettings,
        IRefreshTokenHasher refreshTokenHasher,
        IPersonalAccountService personalAccountService,
        ILogger<AccountService> logger)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
        _refreshTokenHasher = refreshTokenHasher;
        _personalAccountService = personalAccountService;
        _logger = logger;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginUserDto loginUserDto)
    {
        _logger.LogInformation("Attempting to log in user with email: {Email}", loginUserDto.Email);

        var user = await _userRepository.GetUserByLoginAsync(loginUserDto.Email);

        GeneralService.Checker.UniversalCheckException(
            new GeneralService.CheckerParam<Models.User>(new ArgumentException("Incorrect login"),
            x => x[0] == null, user));

        GeneralService.Checker.UniversalCheckException(
            new GeneralService.CheckerParam<Models.User>(new ArgumentException("User was blocked"),
            x => !x[0].IsActive, user));

        var result = new PasswordHasher<Models.User>().VerifyHashedPassword(user, user.PasswordHash, loginUserDto.PasswordHash);

        GeneralService.Checker.UniversalCheckException(
            new GeneralService.CheckerParam<PasswordVerificationResult>(new ArgumentException("Incorrect password"),
            x => x[0] != PasswordVerificationResult.Success, result));

        await _personalAccountService.AddNewLoginHistoryAsync(new NewAuthRecordDto { UserId = user.Id });

        var loginResponseDto = new LoginResponseDto
        {
            UserInfo = _mapper.Map<UserInfoForLoginDto>(user),
            AccessToken = _jwtService.GenerateAccessToken(user),
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresMinutes).ToString(),
            RefreshToken = _mapper.Map<RefreshTokenDto>(await _jwtService.GenerateRefreshTokenAsync(user.Id))
        };

        _logger.LogInformation("User with email: {Email} logged in successfully", loginUserDto.Email);
        
        return loginResponseDto;
    }

    public async Task<AccessTokenResponseDto> CreateAccessTokenAsync(AccessTokenDto accessTokenDto)
    {
        _logger.LogInformation("Attempting to create access token for user ID: {UserId}", accessTokenDto.UserId);
        
        var isValid = await _jwtService.ValidateAccessTokenAsync(accessTokenDto);

        GeneralService.Checker.UniversalCheckException(
            new GeneralService.CheckerParam<AccessTokenDto>(new NullReferenceException("Incorrect token"),
            x => !isValid, accessTokenDto));

        var user = await _userRepository.GetByIdAsync(accessTokenDto.UserId);

        var accessTokenResponseDto = new AccessTokenResponseDto
        {
            UserInfo = _mapper.Map<UserInfoForLoginDto>(user),
            AccessToken = _jwtService.GenerateAccessToken(user),
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresMinutes).ToString()
        };
        
        _logger.LogInformation("Access token created successfully for user ID: {UserId}", accessTokenDto.UserId);
        
        return accessTokenResponseDto;
    }

    public async Task<RefreshTokenResponseDto> CreateRefreshTokenAsync(RefreshTokenRequestDto refreshTokenDto)
    {
        _logger.LogInformation("Attempting to create refresh token for user ID: {UserId}", refreshTokenDto.UserId);
        
        var refreshTokenModel = _mapper.Map<RefreshToken>(refreshTokenDto);
        var isValid = await _jwtService.ValidateRefreshTokenAsync(refreshTokenModel);

        GeneralService.Checker.UniversalCheckException(
            new GeneralService.CheckerParam<RefreshToken>(new NullReferenceException("Incorrect token"),
            x => !isValid, refreshTokenModel));

        var token = _mapper.Map<RefreshTokenDto>(await _jwtService.GenerateRefreshTokenAsync(refreshTokenDto.UserId));
        var refreshTokenResponseDto = _mapper.Map<RefreshTokenResponseDto>(token);

        _logger.LogInformation("Refresh token created successfully for user ID: {UserId}", refreshTokenDto.UserId);
        
        return refreshTokenResponseDto;
    }

    public async Task<RefreshTokenToLoginResponseDto> LoginByRefreshTokenAsync(RefreshTokenToLoginDto refreshToken)
    {
        var token = await _tokenRepository.GetRefreshTokenByValueAsync(_refreshTokenHasher.Hash(refreshToken.RefreshToken));

        GeneralService.Checker.UniversalCheckException(
            new GeneralService.CheckerParam<RefreshToken>(new InvalidOperationException("Incorrect token"),
            x => token == null));

        _logger.LogInformation("Attempting to log in by refresh token for user ID: {UserId}", token.UserId);

        GeneralService.Checker.UniversalCheckException(
            new GeneralService.CheckerParam<RefreshToken>(new InvalidOperationException("Refresh token has expired at " + token.ExpiresAt),
            x => token.ExpiresAt < DateTime.UtcNow));

        await _personalAccountService.AddNewLoginHistoryAsync(new NewAuthRecordDto { UserId = token.UserId });

        var response = new RefreshTokenToLoginResponseDto
        {
            IsAllowed = true
        };

        _logger.LogInformation("User with ID: {UserId} logged in successfully by refresh token", token.UserId);

        return response;
    }
}
