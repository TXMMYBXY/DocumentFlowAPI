using DocumentFlowAPI.Services.Auth.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IAccountService
{
    /// <summary>
    /// Метод для входа в аккаунт
    /// </summary>
    Task<LoginResponseDto> LoginAsync(LoginUserDto loginUserDto);

    /// <summary>
    /// Метод для входа по токену
    /// </summary>
    Task<RefreshTokenToLoginResponseDto> LoginByRefreshTokenAsync(RefreshTokenToLoginDto refreshToken);

    /// <summary>
    /// Метод для создания нового токена обновления
    /// </summary>
    Task<CreateAccessTokenResponseDto> CreateAccessTokenAsync(string token);

    /// <summary>
    /// Метод для создания нового токена доступа
    /// </summary>
    Task<RefreshTokenResponseDto> CreateRefreshTokenAsync(string token);
}