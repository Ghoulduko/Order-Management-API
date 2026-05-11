using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Order.Core.Database;
using Order.Core.Exceptions;
using Order.Core.Interfaces;

namespace Order.Core.Helper;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly OrderDbContext _context;
    private readonly DbSet<T> _dbSet;
    
    public GenericRepository(OrderDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
    
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<T>> FilterAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<bool> CheckExistenceAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }
    
    public async Task<T?> GetSingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.SingleOrDefaultAsync(predicate);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}