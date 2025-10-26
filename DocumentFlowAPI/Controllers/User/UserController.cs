using AutoMapper;
using DocumentFlowAPI.Controllers.User.ViewModels;
using DocumentFlowAPI.Services.User;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<List<UserInfoViewModel>>> GetAllUser()
        {
            return null;
        }

        [HttpPost("{user}/add-user")]
        public async Task<ActionResult<NewUserViewModel>> CreateNewUser([FromRoute] NewUserViewModel user)
        {
            return null;
        }
    }
}