using Order.Application.Dtos.Role;

namespace Order.Application.Interfaces.Role;

public interface IRoleService
{
    Task AddRole(string roleName);
    Task<List<RoleDto>> GetRoles();
    Task<RoleDto> GetRoleById(int id);
    Task DeleteRoleById(int id);
}