using AutoMapper;
using Order.Application.Dtos.User;
using Order.Application.Interfaces;
using Order.Application.Interfaces.Authentication;
using Order.Application.Interfaces.Helper;
using Order.Core.Entities;
using Order.Core.Exceptions;
using Order.Core.Interfaces;

namespace Order.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly ICartRepository _cartRepository;
    private readonly IValidator<AddUserDto> _validator;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    
    public UserService(IUserRepository repository, ICartRepository cartRepository, IValidator<AddUserDto> validator, ITokenService tokenService, IMapper mapper)
    {
        _repository = repository;
        _cartRepository = cartRepository;
        _validator = validator;
        _tokenService = tokenService;
        _mapper = mapper;
    }
    
    public async Task<string> Add(AddUserDto req)
    {
        req.Email = req.Email.Trim().ToLower();
        
        var userExists = await _repository.CheckExistenceAsync(u => u.Email == req.Email);
        if (userExists)
            throw new ArgumentException("the email is already in use");
        
        _validator.Validate(req);
        
        var newUser = new User
        {
            Username = req.Username,
            Email = req.Email.ToLower().Trim(),
            Password = BC.HashPassword(req.Password, 6),
            RoleId = 1,
            IsDeleted = false,
        };
        
        await _repository.AddAsync(newUser);
        await _cartRepository.AddAsync(new Cart { UserId = newUser.Id});

        var userWithRole = await _repository.GetUserWithRoleById(newUser.Id);
        var token = _tokenService.CreateToken(userWithRole!);
        return string.IsNullOrEmpty(token) ? throw new ArgumentException("Token was not generated, try again.") : token;
    }

    public async Task<List<UserDto>> GetAll()
    {
        var users = await _repository.GetAllUsers();
        users = users.Where(u => !u.IsDeleted).ToList();
        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            RoleName = u.Role.Name,
        });
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<List<UserDto>> GetAllDeletedAccounts()
    {
        var users = await _repository.GetAllUsers();
        users = users.Where(u => u.IsDeleted).ToList();
        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            RoleName = u.Role.Name,
        });
        return _mapper.Map<List<UserDto>>(users);
    }
    
    public async Task<UserDto> GetById(int id)
    {
        var user = await _repository.GetUserWithRoleById(id);
        if (user == null)
            throw new NotFoundException("No user found with the provided id");
        
        var userDto = _mapper.Map<UserDto>(user);
        userDto.RoleName = user.Role.Name;
        
        return userDto;
    }
    
    public async Task<UserDto> GetUserByEmail(string email)
    {
        var user = await _repository.GetUserByEmail(email.ToLower().Trim());
        if (user == null || user.IsDeleted)
            throw new NotFoundException("No user found with the provided email");
        
        var userDto = _mapper.Map<UserDto>(user);
        userDto.RoleName = user.Role.Name;
        
        return _mapper.Map<UserDto>(userDto);
    }

    public async Task Delete(int userId)
    {
        var user = await _repository.GetByIdAsync(userId);
        if (user == null || user.IsDeleted)
            throw new NotFoundException("No user found with the provided id");
        user.IsDeleted = true;
        await _cartRepository.ClearCart(userId);
        await _repository.SaveAsync();
    }
}