using Order.Application.Dtos.User;
using Order.Core.Entities;

namespace Order.Application.Interfaces;

public interface IUserService
{
    Task<string> Add(AddUserDto request);
    Task<List<UserDto>> GetAll();
    Task<List<UserDto>> GetAllDeletedAccounts();
    Task<UserDto> GetById(int id);
    Task<UserDto> GetUserByEmail(string email);
    Task Delete(int userId);
}