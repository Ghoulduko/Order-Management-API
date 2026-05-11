using Order.Application.Dtos.User;

namespace Order.Application.Interfaces.Authentication;

public interface IAuthService
{
    Task<string> Login(LoginUserDto req);
}