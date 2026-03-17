using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
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

    public async Task<PersonDto> GetPersonalInfoAsync(int personId)
    {

        return await _dbContext.Users
            .Include(u => u.Role)
            .Select(u => new PersonDto
            {
                FullName = u.FullName,
                Email = u.Email,
                Department = u.Department,
                Role = u.Role
            })
            .AsQueryable()
            .AsNoTracking()
            .SingleAsync();
    }
}