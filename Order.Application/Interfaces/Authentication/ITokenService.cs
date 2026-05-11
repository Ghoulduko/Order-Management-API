using Order.Application.Dtos.User;

namespace Order.Application.Interfaces.Authentication;

public interface ITokenService
{
    string CreateToken(UserDto user);
}