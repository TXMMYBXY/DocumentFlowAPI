using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Repositories.Base;
using DocumentFlowAPI.Services.Personal.Dto;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowAPI.Repositories;

public class PersonalAccountRepository : BaseRepository<Models.User>, IPersonalAccountRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public PersonalAccountRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PersonDto> GetPersonalInfo(int personId)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .Where(u => u.Id == personId)
            .Select(u => new PersonDto
            {
                FullName = u.FullName,
                Email = u.Email,
                Department = u.Department.Title,
                Role = u.Role,
            })
            .AsNoTracking()
            .SingleOrDefaultAsync();
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
}