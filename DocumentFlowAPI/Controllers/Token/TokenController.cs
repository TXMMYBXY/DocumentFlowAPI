using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Controllers.Token
{
    [ApiController]
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        [HttpPost("{userId}/update")]
        public async Task<ActionResult> CreateTokenAsync([FromRoute] int userId)
        {
            return null;
        }
    }
}