using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Repositories.Users;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.General;
using DocumentFlowAPI.Services.Personal.Dto;
using Microsoft.AspNetCore.Identity;

namespace DocumentFlowAPI.Services.Personal;

public class PersonalAccountService : IPersonalAccountService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;

    public PersonalAccountService(IMapper mapper, 
        IUserRepository userRepository,
        IAccountRepository accountRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _accountRepository = accountRepository;
    }
    
    public async Task<GetPersonDto> GetPersonalInfoAsync(int personId)
    {
        var person = await _userRepository.GetPersonalInfo(personId);
        var personDto = _mapper.Map<GetPersonDto>(person);
        
        return personDto;
    }

    public async Task ChangePasswordAsync(int personId, ChangePasswordDto changePasswordDto)
    {
        var person = await _userRepository.GetByIdAsync(personId);
        
        GeneralService.NullCheck(person, "User not found");
        
        var currentPasswordStatus = new PasswordHasher<Models.User>().VerifyHashedPassword(person, person.PasswordHash, changePasswordDto.CurrentPassword);

        GeneralService.Checker.UniversalCheckException(new GeneralService.CheckerParam<PasswordVerificationResult>(new ArgumentException("Incorrect password"),
            x => x[0] != PasswordVerificationResult.Success, currentPasswordStatus));
        
        var passwordsMatching = changePasswordDto.NewPassword.Equals(changePasswordDto.NewPassword);
        
        GeneralService.Checker.UniversalCheckException(new GeneralService.CheckerParam<bool>(new ArgumentException("Passwords not matched"),
            x => x[0] != true, passwordsMatching));
        
        person.PasswordHash = new PasswordHasher<Models.User>().HashPassword(person, changePasswordDto.NewPassword);

        await _userRepository.SaveChangesAsync();
    }

    public async Task<List<GetLoginTimesDto>> GetLoginTimesAsync(int userId)
    {
        var loginTimes = await _accountRepository.GetLoginTimesByUserIdAsync(userId);
        var loginTimesDto = _mapper.Map<List<GetLoginTimesDto>>(loginTimes);
        
        return loginTimesDto;
    }

    public async Task AddNewLoginHistoryAsync(NewAuthRecordDto newAuthRecordDto)
    {
        var loginHistory = _mapper.Map<LoginHistory>(newAuthRecordDto);
        
        var count = await _accountRepository.GetCountOfRecordsByUserIdAsync(newAuthRecordDto.UserId);

        if (count >= 10)
        {
            var lastLoginHistory = await _accountRepository.GetFirstLoginHistoryByUserIdAsync(newAuthRecordDto.UserId);
            
            _accountRepository.Delete(lastLoginHistory);
        }
        
        await _accountRepository.AddNewLoginHistoryAsync(loginHistory);
        await _accountRepository.SaveChangesAsync();
    }
}