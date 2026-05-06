using AutoMapper;
using Order.Application.Dtos.User;
using Order.Application.Interfaces;
using Order.Application.Interfaces.Helper;
using Order.Core.Entities;
using Order.Core.Exceptions;
using Order.Core.Interfaces;

namespace Order.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _repository;
    private readonly IGenericRepository<Cart> _cartService;
    private readonly IValidator<AddUserDto> _validator;
    private readonly IMapper _mapper;
    
    public UserService(IGenericRepository<User> repository, IGenericRepository<Cart> cartService, IValidator<AddUserDto> validator, IMapper mapper)
    {
        _repository = repository;
        _cartService = cartService;
        _validator = validator;
        _mapper = mapper;
    }
    
    public async Task Add(AddUserDto req)
    {
        req.Email = req.Email.Trim().ToLower();
        
        var userExists = await _repository.CheckExistenceAsync(u => u.Email == req.Email);
        if (userExists)
            throw new ArgumentException("the email is already in use");
        
        _validator.Validate(req);
        
        var newUser = new User
        {
            Username = req.Username,
            Email = req.Email,
            Password = BC.HashPassword(req.Password, 6),
        };
        
        
        await _repository.AddAsync(newUser);
        await _cartService.AddAsync(new Cart { UserId = newUser.Id});
    }

    public async Task<UserDto> GetById(int id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null)
            throw new NotFoundException("No user found with the provided id");
        return _mapper.Map<UserDto>(user);
    }
    
    public async Task<UserDto> GetUserByEmail(string email)
    {
        email = email.Trim().ToLower();
        var user = await _repository.GetFirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            throw new NotFoundException("No user found with the provided email");
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
        if (user == null)
            throw new NotFoundException("No user found with the provided id");
        await _repository.DeleteAsync(user);
    }
}