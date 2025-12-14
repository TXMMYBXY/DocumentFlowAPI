using DocumentFlowAPI.Services.Auth.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IAccountService
{
    Task<LoginResponseDto> LoginAsync(LoginUserDto loginUserDto);
    Task<RefreshTokenResponseDto> RefreshAsync(RefreshTokenDto refreshTokenDto);
}