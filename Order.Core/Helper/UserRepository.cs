using Microsoft.EntityFrameworkCore;
using Order.Core.Database;
using Order.Core.Entities;
using Order.Core.Interfaces;

namespace Order.Core.Helper;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(OrderDbContext context) : base(context) {}

    private IQueryable<User> BaseQuery()
    {
        return _context.Users.Include(u => u.Role);
    }
    
    public async Task<List<User>> GetAllUsers()
    {
        return await BaseQuery().ToListAsync();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await BaseQuery().SingleOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<User?> GetUserWithRoleById(int id)
    {
        return await BaseQuery().SingleOrDefaultAsync(u => u.Id == id);
    }
}