using System.Linq.Expressions;
using DocumentFlowAPI.Base;
using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;

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

    public async Task<Models.User> GetUserByIdAsync(int userId)
    {
        return await _dbContext.Users.FindAsync(userId);
    }

    public Models.User UpdateUserInfo(Models.User user)
    {
        UpdateFields(user,
            t => t.FullName,
            t => t.Email,
            t => t.DepartmentId,
            t => t.RoleId);
        return user;
    }

    public Models.User UpdateUserStatus(Models.User user)
    {
        _dbContext.Attach(user);
        _dbContext.Entry(user)
            .Property(t => t.IsActive)
            .IsModified = true;
        return user;
    }
}