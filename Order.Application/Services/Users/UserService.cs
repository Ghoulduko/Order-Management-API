using AutoMapper;
using Order.Application.Dtos.User;
using Order.Application.Interfaces;
using Order.Core.Entities;
using Order.Core.Interfaces;

namespace Order.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _repository;
    private readonly IGenericRepository<Cart> _cartService;
    private readonly IMapper _mapper;
    
    public UserService(IGenericRepository<User> repository, IGenericRepository<Cart> cartService, IMapper mapper)
    {
        _repository = repository;
        _cartService = cartService;
        _mapper = mapper;
    }
    
    public async Task Add(AddUserDto request)
    {
        request.Email = request.Email.Trim().ToLower();
        
        var userExists = await _repository.CheckExistenceAsync(u => u.Email == request.Email);
        
        if (userExists)
            throw new ArgumentException("the email is already in use");
        
        if (request.Username.Length < 3 || request.Username.Length > 10)
            throw new ArgumentException("Username must be between 3 and 10 characters long");
        if (!request.Email.Contains("@") || !request.Email.Contains("."))
            throw new ArgumentException("Email must be a valid email address");
        if (request.Password.Length < 8 || request.Password.Length > 22)
            throw new ArgumentException("Password must be between 8 and 22 characters long");

        var newUser = new User
        {
            Username = request.Username,
            Email = request.Email,
            Password = BC.HashPassword(request.Password, 6),
        };
        
        await _repository.AddAsync(newUser);
        await _cartService.AddAsync(new Cart { UserId = newUser.Id});
    }

    public async Task<UserDto> GetById(int id)
    {
        var user = await _repository.GetByIdAsync(id);
        return _mapper.Map<UserDto>(user);
    }
    
    public async Task<UserDto> GetUserByEmail(string email)
    {
        email = email.Trim().ToLower();
        var user = await _repository.GetFirstAsync(u => u.Email == email);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<List<UserDto>> GetAll()
    {
        var users = await _repository.GetAllAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task Delete(int id)
    {
        var user = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(user);
    }
}