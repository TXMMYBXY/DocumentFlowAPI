using DocumentFlowAPI.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Services.Token;

public class TokenService : ITokenService
{
    public Task<IActionResult> GetTokenById()
    {
        throw new NotImplementedException();
    }
}