using DocumentFlowAPI.Base;
using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DocumentFlowAPI.Repositories.User;

public class UserRepository : BaseRepository<Models.User>, IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateNewUserAsync(Models.User userModel)
    {
        await _dbContext.Users.AddAsync(userModel);
    }
}