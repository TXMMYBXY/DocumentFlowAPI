using AutoMapper;
using DocumentFlowAPI.Configuration;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Models.AboutUserModels;
using DocumentFlowAPI.Services.Auth.Dto;
using DocumentFlowAPI.Services.General;
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
    private readonly IRefreshTokenHasher _refreshTokenHasher;

    public AccountService(
        IMapper mapper,
        IUserRepository userRepository,
        ITokenRepository tokenRepository,
        IJwtService jwtService,
        IOptions<JwtSettings> jwtSettings,
        IRefreshTokenHasher refreshTokenHasher)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
        _refreshTokenHasher = refreshTokenHasher;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginUserDto loginUserDto)
    {
        var user = await _userRepository.GetUserByLoginAsync(loginUserDto.Email);

        Checker.UniversalCheck(new CheckerParam<Models.User>(new ArgumentException("Incorrect login"),
            x => x[0] == null, user));

        Checker.UniversalCheck(new CheckerParam<Models.User>(new ArgumentException("User was deleted"),
            x => !x[0].IsActive, user));

        var result = new PasswordHasher<Models.User>().VerifyHashedPassword(user, user.PasswordHash, loginUserDto.PasswordHash);

        Checker.UniversalCheck(new CheckerParam<PasswordVerificationResult>(new ArgumentException("Incorrect password"),
            x => x[0] != PasswordVerificationResult.Success, result));


        return new LoginResponseDto
        {
            UserInfo = _mapper.Map<UserInfoForLoginDto>(user),
            AccessToken = _jwtService.GenerateAccessToken(user),
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresMinutes).ToString(),
            RefreshToken = await _jwtService.GenerateRefreshTokenAsync(user.Id)
        };
    }

    public async Task<AccessTokenResponseDto> CreateAccessTokenAsync(AccessTokenDto accessTokenDto)
    {
        var isValid = await _jwtService.ValidateAccessTokenAsync(accessTokenDto);

        Checker.UniversalCheck(new CheckerParam<AccessTokenDto>(new NullReferenceException("Incorrect token"),
            x => !isValid == true, accessTokenDto));

        var user = await _userRepository.GetUserByIdAsync(accessTokenDto.UserId);

        return new AccessTokenResponseDto
        {
            UserInfo = _mapper.Map<UserInfoForLoginDto>(user),
            AccessToken = _jwtService.GenerateAccessToken(user),
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresMinutes).ToString(),
            RefreshToken = await _tokenRepository.GetRefreshTokenByUserIdAsync(user.Id)
        };
    }

    public async Task<RefreshTokenResponseDto> CreateRefreshTokenAsync(RefreshTokenDto refreshTokenDto)
    {
        var refreshTokenModel = _mapper.Map<RefreshToken>(refreshTokenDto);
        var isValid = await _jwtService.ValidateRefreshTokenAsync(refreshTokenModel);

        Checker.UniversalCheck(new CheckerParam<RefreshToken>(new NullReferenceException("Incorrect token"),
            x => !isValid == true, refreshTokenModel));

        var refreshToken = await _tokenRepository.GetRefreshTokenByUserIdAsync(refreshTokenDto.UserId);
        var token = await _jwtService.GenerateRefreshTokenAsync(refreshTokenDto.UserId);
        var refreshTokenResponseDto = _mapper.Map<RefreshTokenResponseDto>(token);

        await _tokenRepository.SaveChangesAsync();

        return refreshTokenResponseDto;
    }

    public async Task<RefreshTokenToLoginResponseDto> LoginByRefreshTokenAsync(RefreshTokenToLoginDto refreshToken)
    {
        var token = await _tokenRepository.GetRefreshTokenByValueAsync(_refreshTokenHasher.Hash(refreshToken.RefreshToken));

        Checker.UniversalCheck(new CheckerParam<RefreshToken>(new NullReferenceException("Incorrect token"),
            x => token == null));

        var response = new RefreshTokenToLoginResponseDto
        {
            IsAllowed = true
        };

        return response;
    }
}
