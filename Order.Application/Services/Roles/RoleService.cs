using AutoMapper;
using Order.Application.Dtos.Role;
using Order.Application.Interfaces.Role;
using Order.Core.Entities;
using Order.Core.Exceptions;
using Order.Core.Interfaces;

namespace Order.Application.Services.Roles;

public class RoleService : IRoleService
{
    private readonly IGenericRepository<Role> _roleRepository;
    private readonly IMapper _mapper;
    
    public RoleService(IGenericRepository<Role> roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task AddRole(string roleName)
    {
        if (string.IsNullOrEmpty(roleName))
            throw new InvalidRoleNameException("The role name cannot be null or empty.");
        
        var role = new Role()
        {
            Name = roleName.ToUpper().Trim()
        };
        
        await _roleRepository.AddAsync(role);
    }

    public async Task<List<RoleDto>> GetRoles()
    {
        var roles = await _roleRepository.GetAllAsync();
        return _mapper.Map<List<RoleDto>>(roles);
    }

    public async Task<RoleDto> GetRoleById(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        return role == null ? throw new RoleNotFoundException("The role could not be found.") : _mapper.Map<RoleDto>(role);
    }

    public async Task DeleteRoleById(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role == null)
            throw new RoleNotFoundException("The role could not be found.");
        await _roleRepository.DeleteAsync(role);
    }
}