using System.Linq.Expressions;

namespace DocumentFlowAPI.Base
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
        Task<T> GetEntityById(int id);
        void UpdateFields(T entity, params Expression<Func<T, object>>[] fields);
    }
}