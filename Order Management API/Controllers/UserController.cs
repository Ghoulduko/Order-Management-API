using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Dtos.User;
using Order.Application.Interfaces;
using Order.Application.Interfaces.Authentication;

namespace Order_Management_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public UserController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost("Register")]
    public async Task<Ok<string>> RegisterUser([FromBody] AddUserDto user)
    {
        var token = await _userService.Add(user);
        return TypedResults.Ok(token);
    }

    [HttpPost("Login")]
    public async Task<Ok<string>> Login([FromBody] LoginUserDto user)
    {
        var token = await _authService.Login(user);
        return TypedResults.Ok(token);
    }

    [HttpGet("GetAllUsers")]
    [Authorize]
    public async Task<Ok<List<UserDto>>> GetAllUsers()
    {
        var allUsers = await _userService.GetAll();
        return TypedResults.Ok(allUsers);
    }
  
    [HttpGet("GetAllDeletedAccounts")]
    [Authorize(Roles = "OWNER,SUPERADMIN,ADMIN")]
    public async Task<Ok<List<UserDto>>> GetAllDeletedAccounts()
    {
        var allUsers = await _userService.GetAllDeletedAccounts();
        return TypedResults.Ok(allUsers);
    }

    [HttpGet("GetUserProfile")]
    [Authorize]
    public async Task<Ok<UserDto>> GetLoggedInUserProfile()
    {
        var userId = User.FindFirst("UserId")?.Value ?? throw new UnauthorizedAccessException("You need to login first");
        var user = await _userService.GetById(int.Parse(userId));
        return TypedResults.Ok(user);
    }

    [HttpGet("GetUserById/{id}")]
    [Authorize(Roles = "OWNER,SUPERADMIN,ADMIN")]
    public async Task<Ok<UserDto>> GetUserById(int id)
    {
        var user = await _userService.GetById(id);
        return TypedResults.Ok(user);
    }

    [HttpGet("GetUserByEmail/{email}")]
    [Authorize(Roles = "OWNER,SUPERADMIN,ADMIN")]
    public async Task<Ok<UserDto>> GetUserByEmail(string email)
    {
        var user = await _userService.GetUserByEmail(email);
        return TypedResults.Ok(user);
    }

    [HttpDelete("DeleteAccount")]
    [Authorize]
    public async Task<Ok> DeleteAccount()
    {
        var userId = User.FindFirst("UserId")?.Value ?? throw new UnauthorizedAccessException("You need to login first");
        await _userService.Delete(int.Parse(userId));
        return TypedResults.Ok();
    }
    
}