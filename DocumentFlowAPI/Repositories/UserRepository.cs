using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories.Users;
using DocumentFlowAPI.Interfaces.Repositories.Users.Dtos;
using DocumentFlowAPI.Repositories.Base;
using DocumentFlowAPI.Services.Personal.Dto;
using DocumentFlowAPI.Services.Role.Dto;
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

    public async Task<Models.User> GetUserByLoginAsync(string login)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email.Equals(login));
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
        var query = _dbContext.Users
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Email))
            query = query.Where(u => u.Email.Contains(filter.Email));

        if (!string.IsNullOrWhiteSpace(filter.FullName))
            query = query.Where(u => u.FullName.Contains(filter.FullName));

        if (filter.DepartmentId.HasValue)
            query = query.Where(u => u.DepartmentId == filter.DepartmentId);

        if (filter.RoleId.HasValue)
            query = query.Where(u => u.RoleId == filter.RoleId);

        var reusltQuery = query
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Department = u.Department,
                IsActive = u.IsActive,
                Role = u.Role
            });

        if (filter.PageSize.HasValue && filter.PageNumber.HasValue)
        {
            reusltQuery = reusltQuery
                .Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value);
        }

        return await reusltQuery.ToListAsync();
    }

    public async Task<bool> UpdateUserStatusAsync(int userId)
    {
        await _dbContext.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(
                setter => setter.SetProperty(x => x.IsActive, x => !x.IsActive)
            );

        return await _dbContext.Users
            .Where(u => u.Id == userId)
            .Select(u => u.IsActive)
            .FirstAsync();
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

    public async Task<int> GetTotalCountAsync()
    {
        return await _dbContext.Users.CountAsync();
    }

    public async Task DeleteManyAsync(List<int> ids)
    {
        var users = await _dbContext.Users.Where(u => ids.Contains(u.Id)).ToListAsync();

        _dbContext.Users.RemoveRange(users);
    }

    public async Task<UserInfoDto> GetUserInfoByIdAsync(int userId)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .Where(u => u.Id == userId)
            .Select(u => new UserInfoDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Department = u.Department.Title,
                Role = u.Role.Title,
            })
            .AsNoTracking()
            .SingleOrDefaultAsync();
    }
}