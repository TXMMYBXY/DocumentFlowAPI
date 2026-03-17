using AutoMapper;
using DocumentFlowAPI.Controllers.PersonalAccount.ViewModels;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.Personal.Dto;
using DocumentFlowAPI.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Controllers.PersonalAccount;

[ApiController]
[Route("api/personal")]
[Authorize]
public class PersonalAccountController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPersonalAccountService _personalAccountService;

    public PersonalAccountController(IMapper mapper, IPersonalAccountService personalAccountService)
    {
        _mapper = mapper;
        _personalAccountService = personalAccountService;
    }

    [HttpGet]
    public async Task<ActionResult<GetPersonViewModel>> GetPersonalInfo()
    {
        var personDto = await _personalAccountService.GetPersonalInfoAsync(UserIdentity.User!.Id);
        var personViewModel = _mapper.Map<GetPersonViewModel>(personDto);

        return Ok(personViewModel);
    }

    [HttpPatch("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordViewModel  changePasswordViewModel)
    {
        var changePasswordDto = _mapper.Map<ChangePasswordDto>(changePasswordViewModel);
        
        await _personalAccountService.ChangePasswordAsync(UserIdentity.User.Id, changePasswordDto);
        
        return Ok();
    }
}