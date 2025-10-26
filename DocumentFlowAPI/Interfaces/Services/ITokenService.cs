using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Interfaces.Services
{
    public interface ITokenService
    {
        Task<IActionResult> GetTokenById();
    }
}