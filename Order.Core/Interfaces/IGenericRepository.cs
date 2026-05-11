using System.Linq.Expressions;

namespace Order.Core.Interfaces;

public interface IGenericRepository<T>
{
    Task AddAsync(T entity);
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task DeleteAsync(T entity);
    Task<List<T>> FilterAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetSingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<bool> CheckExistenceAsync(Expression<Func<T, bool>> predicate);
    Task SaveAsync();
}