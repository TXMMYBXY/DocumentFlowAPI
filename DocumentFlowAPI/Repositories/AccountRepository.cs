using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Repositories.Base;
using DocumentFlowAPI.Services.Personal.Dto;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowAPI.Repositories;

public class AccountRepository : BaseRepository<LoginHistory>, IAccountRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public AccountRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<LoginTimeDto>> GetLoginTimesByUserIdAsync(int userId)
    {
        return await _dbContext.LoginHistories
            .Where(l => l.UserId == userId)
            .Select(l => new LoginTimeDto()
            {
                LoginTime = l.LoginDate
            })
            .OrderByDescending(l => l.LoginTime)
            .Take(10)
            .ToListAsync();
    }

    public async Task AddNewLoginHistoryAsync(LoginHistory loginHistory)
    {
        await _dbContext.LoginHistories.AddAsync(loginHistory);
    }

    public async Task<int> GetCountOfRecordsByUserIdAsync(int userId)
    {
        return await _dbContext.LoginHistories.CountAsync(l => l.UserId == userId);
    }

    public async Task<LoginHistory> GetFirstLoginHistoryByUserIdAsync(int userId)
    {
        return await _dbContext.LoginHistories.FirstOrDefaultAsync(l => l.UserId == userId);
    }
}