using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Services.Token
{
    public interface ITokenService
    {
        Task<IActionResult> GetTokenById();
    }
}