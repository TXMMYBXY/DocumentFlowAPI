using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.General;
using DocumentFlowAPI.Services.Personal.Dto;
using Microsoft.AspNetCore.Identity;

namespace DocumentFlowAPI.Services.Personal;

public class PersonalAccountService : IPersonalAccountService
{
    private readonly IMapper _mapper;
    private readonly IPersonalAccountRepository _personalAccountRepository;

    public PersonalAccountService(IMapper mapper, IPersonalAccountRepository personalAccountRepository)
    {
        _mapper = mapper;
        _personalAccountRepository = personalAccountRepository;
    }
    
    public async Task<GetPersonDto> GetPersonalInfoAsync(int personId)
    {
        var person = await _personalAccountRepository.GetPersonalInfo(personId);
        var personDto = _mapper.Map<GetPersonDto>(person);
        
        return personDto;
    }

    public async Task ChangePasswordAsync(int personId, ChangePasswordDto changePasswordDto)
    {
        var person = await _personalAccountRepository.GetByIdAsync(personId);
        
        GeneralService.NullCheck(person, "User not found");
        
        var currentPasswordStatus = new PasswordHasher<Models.User>().VerifyHashedPassword(person, person.PasswordHash, changePasswordDto.CurrentPassword);

        GeneralService.Checker.UniversalCheckException(new GeneralService.CheckerParam<PasswordVerificationResult>(new ArgumentException("Incorrect password"),
            x => x[0] != PasswordVerificationResult.Success, currentPasswordStatus));
        
        var passwordsMatching = changePasswordDto.NewPassword.Equals(changePasswordDto.NewPassword);
        
        GeneralService.Checker.UniversalCheckException(new GeneralService.CheckerParam<bool>(new ArgumentException("Passwords not matched"),
            x => x[0] != true, passwordsMatching));
        
        person.PasswordHash = new PasswordHasher<Models.User>().HashPassword(person, changePasswordDto.NewPassword);

        await _personalAccountRepository.SaveChangesAsync();
    }

    public async Task<List<GetLoginTimesDto>> GetLoginTimesAsync(int userId)
    {
        var loginTimes = await _personalAccountRepository.GetLoginTimesByUserIdAsync(userId);
        var loginTimesDto = _mapper.Map<List<GetLoginTimesDto>>(loginTimes);
        
        return loginTimesDto;
    }

    public async Task AddNewLoginHistoryAsync(NewAuthRecordDto newAuthRecordDto)
    {
        var loginHistory = _mapper.Map<LoginHistory>(newAuthRecordDto);
        
        await _personalAccountRepository.AddNewLoginHistoryAsync(loginHistory);
    }
}