using Order.Core.Entities;

namespace Order.Application.Interfaces.Authentication;

public interface ITokenService
{
    string CreateToken(User user);
}