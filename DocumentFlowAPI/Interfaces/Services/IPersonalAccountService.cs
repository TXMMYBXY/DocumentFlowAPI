using DocumentFlowAPI.Services.Personal.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IPersonalAccountService
{
    Task<GetPersonDto> GetPersonalInfoAsync(int personId);
    Task ChangePasswordAsync(int personId, ChangePasswordDto changePasswordDto);
    Task<List<GetLoginTimesDto>> GetLoginTimesAsync(int userId);
    Task AddNewLoginHistoryAsync(NewAuthRecordDto newAuthRecordDto);
}