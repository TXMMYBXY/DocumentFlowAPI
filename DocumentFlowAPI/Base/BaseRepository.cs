using System.Linq.Expressions;
using DocumentFlowAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowAPI.Base;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext;
    protected readonly DbSet<T> _dbset;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbset = dbContext.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbset.AddAsync(entity);
    }

    public void Delete(T entity)
    {
        _dbset.Remove(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbset.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbset.FindAsync(id);
    }

    public async Task<T> GetEntityById(int id)
    {
        return await _dbset.FindAsync(id);
    }

    /// <summary>
    /// Метод для сохранения изменений в БД
    /// </summary>
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        _dbset.Update(entity);
    }

    public void UpdateFields(T entity, params Expression<Func<T, object>>[] fields)
    {
        _dbContext.Attach(entity);

        foreach (var field in fields)
        {
            _dbContext.Entry(entity).Property(field).IsModified = true;
        }
    }
}