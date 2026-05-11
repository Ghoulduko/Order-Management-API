using AutoMapper;
using Order.Application.Dtos.User;
using Order.Application.Interfaces;
using Order.Application.Interfaces.Authentication;
using Order.Application.Services.Notifications;
using Order.Core.Entities;
using Order.Core.Exceptions;
using Order.Core.Interfaces;

namespace Order.Application.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<User> _userService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly EmailNotificationObserver _emailService;
    
    public AuthService(IGenericRepository<User> userService, ITokenService tokenService, IMapper mapper, EmailNotificationObserver emailService)
    {
        _userService = userService;
        _tokenService = tokenService;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<string> Login(LoginUserDto req)
    {
        var user = await _userService.GetSingleOrDefaultAsync(u => u.Email == req.Email);
        if (user == null || user.IsDeleted)
            throw new ArgumentException($"User with email {req.Email} not found");
        
        if (!BC.Verify(req.Password, user.Password))
            throw new IncorrectPasswordException("Password is wrong, try again.");
        
        var userDto = _mapper.Map<UserDto>(user);
        
        var token = _tokenService.CreateToken(userDto);

        await _emailService.OnLogin(user.Email, user.Username);
        
        return string.IsNullOrEmpty(token) ? throw new ArgumentException("Token was not generated, try again.") : token;
    }
    
}