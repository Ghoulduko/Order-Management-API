using Order.Application.Dtos.User;

namespace Order.Application.Interfaces;

public interface IUserService
{
    Task Add(AddUserDto request);
    Task<UserDto> GetById(int id);
    Task<UserDto> GetUserByEmail(string email);
    Task<List<UserDto>> GetAll();
    Task<List<UserDto>> GetAllDeletedAccounts();
    Task Delete(int id);
}