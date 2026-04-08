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
    private readonly ILogger<PersonalAccountService> _logger;

    public PersonalAccountService(IMapper mapper,
        IUserRepository userRepository,
        IAccountRepository accountRepository,
        ILogger<PersonalAccountService> logger)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public async Task<GetPersonDto> GetPersonalInfoAsync(int personId)
    {
        var person = await _userRepository.GetPersonalInfo(personId);
        var personDto = _mapper.Map<GetPersonDto>(person);

        return personDto;
    }

    public async Task ChangePasswordAsync(int personId, ChangePasswordDto changePasswordDto)
    {
        _logger.LogInformation("Changing password for user with ID {PersonId}", personId);
        
        var person = await _userRepository.GetByIdAsync(personId);

        GeneralService.NullCheck(person, "User not found");

        var currentPasswordStatus = new PasswordHasher<Models.User>().VerifyHashedPassword(person, person.PasswordHash, changePasswordDto.CurrentPassword);

        GeneralService.Checker.UniversalCheckException(new GeneralService.CheckerParam<PasswordVerificationResult>(new ArgumentException("Incorrect password"),
            x => x[0] != PasswordVerificationResult.Success, currentPasswordStatus));

        var passwordsMatching = changePasswordDto.NewPassword.Equals(changePasswordDto.NewPassword);

        GeneralService.Checker.UniversalCheckException(new GeneralService.CheckerParam<bool>(new ArgumentException("Passwords not matched"),
            x => !x[0], passwordsMatching));

        person.PasswordHash = new PasswordHasher<Models.User>().HashPassword(person, changePasswordDto.NewPassword);

        await _userRepository.SaveChangesAsync();

        _logger.LogInformation("Password changed successfully for user with ID {PersonId}", personId);
    }

    public async Task<List<GetLoginTimesDto>> GetLoginTimesAsync(int userId)
    {
        var loginTimes = await _accountRepository.GetLoginTimesByUserIdAsync(userId);
        var loginTimesDto = _mapper.Map<List<GetLoginTimesDto>>(loginTimes);

        return loginTimesDto;
    }

    public async Task AddNewLoginHistoryAsync(NewAuthRecordDto newAuthRecordDto)
    {
        _logger.LogInformation("Adding new login history for user with ID {UserId}", newAuthRecordDto.UserId);
        
        var loginHistory = _mapper.Map<LoginHistory>(newAuthRecordDto);

        var count = await _accountRepository.GetCountOfRecordsByUserIdAsync(newAuthRecordDto.UserId);

        if (count >= 10)
        {
            var lastLoginHistory = await _accountRepository.GetFirstLoginHistoryByUserIdAsync(newAuthRecordDto.UserId);

            await _accountRepository.DeleteAsync(lastLoginHistory);
        }

        await _accountRepository.AddNewLoginHistoryAsync(loginHistory);
        await _accountRepository.SaveChangesAsync();

        _logger.LogInformation("New login history added successfully for user with ID {UserId}", newAuthRecordDto.UserId);
    }
}