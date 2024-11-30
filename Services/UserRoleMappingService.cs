using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class UserRoleMappingService : IUserRoleMappingService
{
    private readonly OrderPickingContext _context;

    public UserRoleMappingService(OrderPickingContext context)
    {
        _context = context;
    }

    public UserRoleMapping CreateUserRoleMapping(int userId, UserRole role)
    {
        var userRoleMapping = new UserRoleMapping
        {
            UserId = userId,
            Role = role
        };

        _context.UserRoleMappings.Add(userRoleMapping);

        return userRoleMapping;
    }

    private async Task<UserRoleMapping> QueryUserRoleMapping(int userId, UserRole role)
    {
        var userRoleMapping =
            await _context.UserRoleMappings.FirstOrDefaultAsync(urm => urm.UserId == userId && urm.Role == role);

        if (userRoleMapping == null)
            throw new ArgumentException("Role not found");

        return userRoleMapping;
    }

    public void RemoveUserRoleMapping(UserRoleMapping userRoleMapping)
        => _context.UserRoleMappings.Remove(userRoleMapping);
}