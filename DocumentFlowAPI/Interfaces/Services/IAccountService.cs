using DocumentFlowAPI.Services.Auth.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IAccountService
{
    /// <summary>
    /// Метод для входа в аккаунт
    /// </summary>
    Task<LoginResponseDto> LoginAsync(LoginUserDto loginUserDto);

    /// <summary>
    /// Метод для создания нового токена доступа
    /// </summary>
    Task<RefreshTokenResponseDto> RefreshAsync(RefreshTokenDto refreshTokenDto);
    
    /// <summary>
    /// Метод для создания нового токена обновления
    /// </summary>
    Task<AccessTokenResponseDto> CreateAccessTokenAsync(AccessTokenDto accessTokenDto);
}