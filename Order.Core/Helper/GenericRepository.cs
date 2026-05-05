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
    
    public async Task<T> GetByIdAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            throw new NotFoundException($"{typeof(T).Name} with id {id} not found");
        return entity;
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
    
    public async Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(predicate);
        if (entity == null)
            throw new NotFoundException($"{typeof(T).Name} not found");
        return entity;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}