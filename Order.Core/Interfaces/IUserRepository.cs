using Order.Core.Entities;

namespace Order.Core.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<List<User>> GetAllUsers();
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserWithRoleById(int id);
}