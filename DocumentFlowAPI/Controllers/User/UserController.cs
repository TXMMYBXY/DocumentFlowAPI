using AutoMapper;
using Castle.Components.DictionaryAdapter.Xml;
using DocumentFlowAPI.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DocumentFlowAPI.Controllers.User
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }
        
        [HttpGet("/all")]
        public async Task<ActionResult<Models.User>> GetAllUser()
        {
            return null;
        }
    }
}