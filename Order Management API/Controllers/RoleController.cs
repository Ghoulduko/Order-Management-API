using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Dtos.Role;
using Order.Application.Interfaces.Role;

namespace Order_Management_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("AddRole")]
    [Authorize(Roles = "OWNER,SUPERADMIN")]
    public async Task<Ok<string>> AddRole(string roleName)
    {
        await _roleService.AddRole(roleName);
        return TypedResults.Ok($"successfully added role {roleName.ToUpper()}");
    }

    [HttpGet("GetRoles")]
    [Authorize(Roles = "OWNER,SUPERADMIN,ADMIN")]
    public async Task<Ok<List<RoleDto>>> GetRoles()
    {
        var roles = await _roleService.GetRoles();
        return TypedResults.Ok(roles);
    }

    [HttpGet("GetRoleById/{id}")]
    [Authorize(Roles = "OWNER,SUPERADMIN,ADMIN")]
    public async Task<Ok<RoleDto>> GetRoleById(int id)
    {
        var role = await _roleService.GetRoleById(id);
        return TypedResults.Ok(role);
    }

    [HttpDelete("DeleteRoleById/{id}")]
    [Authorize(Roles = "OWNER,SUPERADMIN")]
    public async Task<Ok<string>> DeleteRoleById(int id)
    {
        await _roleService.DeleteRoleById(id);
        return TypedResults.Ok($"successfully deleted role {id}");
    }
}