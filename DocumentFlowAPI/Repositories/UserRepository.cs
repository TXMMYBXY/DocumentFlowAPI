using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Repositories.Users;
using DocumentFlowAPI.Interfaces.Repositories.Users.Dtos;
using DocumentFlowAPI.Repositories.Base;
using DocumentFlowAPI.Services.User;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowAPI.Repositories;

public class UserRepository : BaseRepository<Models.User>, IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Models.User> GetUserByLoginAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));
    }

    /// <summary>
    /// Проверка на существующий логин
    /// </summary>
    /// <param name="email"></param>
    /// <returns>Возвращает true, если логин уже используется</returns>
    public async Task<bool> IsUserAlreadyExists(string email)
    {
        return await _dbContext.Users.AnyAsync(x => x.Email.Equals(email));
    }

    public async Task<List<UserDto>> GetAllUsersAsync(UserFilter filter)
    {
        var  query = _dbContext.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Department = u.Department,
                RoleId = u.RoleId
            })
            .AsQueryable();

        if (filter != null)
        {
            if(!string.IsNullOrWhiteSpace(filter.Email)) query = query.Where(u => u.Email.Contains(filter.Email));
            if (!string.IsNullOrWhiteSpace(filter.FullName)) query = query.Where(u => u.FullName.Contains(filter.FullName));
            if (!string.IsNullOrWhiteSpace(filter.Department)) query = query.Where(u => u.Department.Contains(filter.Department));
            if (filter.RoleId.HasValue) query = query.Where(u => u.RoleId == filter.RoleId);
            
            if (filter.PageSize.HasValue && filter.PageNumber.HasValue)
            {
                if (filter.PageNumber.HasValue) query = query
                    .Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                    .Take(filter.PageSize.Value);
            }
        }
        
        return await query.ToListAsync();
    }

    public Models.User UpdateUser(Models.User userModel)
    {
        UpdateFields(userModel,
            t => t.FullName,
            t => t.Email,
            t => t.Department,
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