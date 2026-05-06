using Order.Application.Dtos.User;
using Order.Application.Interfaces.Helper;

namespace Order.Application.Services.Users;

public class AddUserValidator : IValidator<AddUserDto>
{
    public void Validate(AddUserDto req)
    {
        if (req.Username.Length < 3 || req.Username.Length > 10)
            throw new ArgumentException("Username must be between 3 and 10 characters long");
        if (!req.Email.Contains("@") || !req.Email.Contains("."))
            throw new ArgumentException("Email must be a valid email address");
        if (req.Password.Length < 8 || req.Password.Length > 22)
            throw new ArgumentException("Password must be between 8 and 22 characters long");
    }
}