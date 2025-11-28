using DocumentFlowAPI.Base;
using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Models.User> GetUserByLoginAsync(string login)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Login.Equals(login));
    }

    /// <summary>
    /// Проверка на сущесмтвующий логин
    /// </summary>
    /// <param name="login"></param>
    /// <returns>Возвращает true, если логин уже используется</returns>
    public async Task<bool> IsUserAlreadyExists(string login)
    {
        return await _dbContext.Users.AnyAsync(x => x.Login.Equals(login));
    }

    public async Task RegisterUserAsync(Models.User userModel)
    {
        await _dbContext.Users.AddAsync(userModel);
    }

    public Models.User UpdateUserInfo(Models.User userModel)
    {
        UpdateFields(userModel,
            t => t.FullName,
            t => t.Email,
            t => t.DepartmentId,
            t => t.RoleId);
        return userModel;
    }

    public Models.User UpdateUserStatus(Models.User userModel)
    {
        _dbContext.Attach(userModel);
        _dbContext.Entry(userModel)
            .Property(t => t.IsActive)
            .IsModified = true;
        return userModel;
    }
}