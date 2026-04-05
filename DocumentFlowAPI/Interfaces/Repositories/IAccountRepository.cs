using DocumentFlowAPI.Interfaces.Base;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.Personal.Dto;

namespace DocumentFlowAPI.Interfaces.Repositories;

public interface IAccountRepository : IBaseRepository<LoginHistory>
{
    Task<List<LoginTimeDto>> GetLoginTimesByUserIdAsync(int userId);
    Task AddNewLoginHistoryAsync(LoginHistory loginHistory);
    Task<int> GetCountOfRecordsByUserIdAsync(int userId);
    Task<int> GetFirstLoginHistoryByUserIdAsync(int userId);
}