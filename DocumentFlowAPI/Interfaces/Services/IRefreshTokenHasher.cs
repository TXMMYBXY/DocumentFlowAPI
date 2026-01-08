namespace DocumentFlowAPI.Interfaces.Services;

public interface IRefreshTokenHasher
{
    string Hash(string refreshToken);
}
