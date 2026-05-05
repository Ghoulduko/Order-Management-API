using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Dtos.User;
using Order.Application.Interfaces;

namespace Order_Management_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Register")]
    public async Task<Ok> AddUser([FromBody] AddUserDto user)
    {
        await _userService.Add(user);
        return TypedResults.Ok();
    }

    [HttpGet("GetAllUsers")]
    public async Task<Ok<List<UserDto>>> GetAllUsers()
    {
        var allUsers = await _userService.GetAll();
        return TypedResults.Ok(allUsers);
    }

    [HttpGet("GetUserById/{id}")]
    public async Task<Ok<UserDto>> GetUserById(int id)
    {
        var user = await _userService.GetById(id);
        return TypedResults.Ok(user);
    }

    [HttpGet("GetUserByEmail/{email}")]
    public async Task<Ok<UserDto>> GetUserByEmail(string email)
    {
        var user = await _userService.GetUserByEmail(email);
        return TypedResults.Ok(user);
    }

    [HttpDelete("DeleteUser/{id}")]
    public async Task<Ok> DeleteUser(int id)
    {
        await _userService.Delete(id);
        return TypedResults.Ok();
    }
    
}