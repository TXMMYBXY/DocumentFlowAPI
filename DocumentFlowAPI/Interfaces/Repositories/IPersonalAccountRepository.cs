using DocumentFlowAPI.Interfaces.Base;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.Personal.Dto;

namespace DocumentFlowAPI.Interfaces.Repositories;

public interface IPersonalAccountRepository : IBaseRepository<Models.User>
{
    Task<PersonDto> GetPersonalInfo(int personId);
    Task<List<LoginTimeDto>> GetLoginTimesByUserIdAsync(int userId);
    Task AddNewLoginHistoryAsync(LoginHistory loginHistory);
}