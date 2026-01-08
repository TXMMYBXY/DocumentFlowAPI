using System.Security.Cryptography;
using System.Text;
using DocumentFlowAPI.Configuration;
using DocumentFlowAPI.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace DocumentFlowAPI.Services.Auth;

public sealed class RefreshTokenHasher : IRefreshTokenHasher
{
    private readonly RefreshTokenSettings _refreshTokenSettings;

    public RefreshTokenHasher(IOptions<RefreshTokenSettings> refreshTokenSettings)
    {
        _refreshTokenSettings = refreshTokenSettings.Value;
    }

    public string Hash(string refreshToken)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_refreshTokenSettings.SecretKey));
        var hashBytes = hmac.ComputeHash(
            Encoding.UTF8.GetBytes(refreshToken)
        );

        return Convert.ToBase64String(hashBytes);
    }
}
