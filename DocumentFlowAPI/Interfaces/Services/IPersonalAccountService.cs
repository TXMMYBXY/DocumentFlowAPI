using DocumentFlowAPI.Services.Personal.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IPersonalAccountService
{
    Task<GetPersonDto> GetPersonalInfoAsync(int personId);
}